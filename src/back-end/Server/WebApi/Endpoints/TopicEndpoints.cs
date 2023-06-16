﻿using Carter;
using Core.Collections;
using Core.DTO.Topic;
using Core.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Apps.Departments;
using Services.Apps.Lecturers;
using Services.Apps.Others;
using Services.Apps.Students;
using Services.Apps.Topics;
using Services.Media;
using SlugGenerator;
using System.Net;
using WebApi.Filters;
using WebApi.Models;
using WebApi.Models.Topic;

namespace WebApi.Endpoints
{
    public class TopicEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var routeGroupBuilder = app.MapGroup("/api/topics");

            routeGroupBuilder.MapGet("/", GetTopics)
                .WithName("GetTopics")
                .Produces<ApiResponse<PaginationResult<TopicDto>>>();

            routeGroupBuilder.MapGet("/all", GetAllTopic)
                .WithName("GetAllTopic")
                .Produces<ApiResponse<PaginationResult<TopicDto>>>();

            routeGroupBuilder.MapGet("/done", GetDoneTopics)
                .WithName("GetDoneTopics")
                .Produces<ApiResponse<PaginationResult<TopicDto>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTopicById)
                  .WithName("GetTopicById")
                  .Produces<ApiResponse<TopicDto>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetTopicBySlug)
                  .WithName("GetTopicBySlug")
                  .Produces<ApiResponse<TopicDto>>();

            routeGroupBuilder.MapPost("/", AddTopic)
                .WithName("AddTopic")
                .Accepts<TopicEditModel>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<TopicItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTopic)
                .WithName("UpdateTopic")
                .AddEndpointFilter<ValidatorFilter<TopicEditModel>>()
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTopic)
                .WithName("DeleteTopic")
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
                .WithName("GetTopicFilter")
                .Produces<ApiResponse<TopicFilterModel>>();

            routeGroupBuilder.MapPost("/register/{id:int}", RegisterTopic)
                  .WithName("RegisterTopic")
                  .AddEndpointFilter<ValidatorFilter<TopicAddStudent>>()
                  .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPost("/assignment", SetTopicLecturer)
                  .WithName("SetTopicLecturer")
                  .Accepts<TopicAddLecturer>("multipart/form-data")
                  .Produces(401)
                  .Produces<ApiResponse<TopicItem>>();

            routeGroupBuilder.MapPost("/outlineFile/{slug:regex(^[a-z0-9_-]+$)}", SetTopicOutline)
              .WithName("SetTopicOutline")
              .Accepts<IFormFile>("multipart/form-data")
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPost("/resultFile/{slug:regex(^[a-z0-9_-]+$)}", SetTopicResult)
              .WithName("SetTopicResult")
              .Accepts<IFormFile>("multipart/form-data")
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/view/{slug:regex(^[a-z0-9_-]+$)}", IncreaseView)
              .WithName("IncreaseView")
              .Produces<ApiResponse<string>>();
        }

        private static async Task<IResult> GetAllTopic(
            [FromServices] ITopicRepository topicRepository)
        {
            var topics = await topicRepository.GetTopicsAsync();
            return Results.Ok(ApiResponse.Success(topics));
        }

        private static async Task<IResult> GetTopics(
            [AsParameters] TopicFilterModel model,
            ITopicRepository topicRepository,
            IMapper mapper)
        {
            var query = mapper.Map<TopicQuery>(model);
            var topics = await topicRepository.GetPagedTopicsAsync<TopicDto>(query, model,
                topics => topics.ProjectToType<TopicDto>());
            var paginationResult = new PaginationResult<TopicDto>(topics);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetDoneTopics(
            [AsParameters] TopicFilterModel model,
            ITopicRepository topicRepository,
            IMapper mapper)
        {
            var query = mapper.Map<TopicQuery>(model);
            var topics = await topicRepository.GetPagedDoneTopicsAsync<TopicDto>(query, model,
                topics => topics.ProjectToType<TopicDto>());
            var paginationResult = new PaginationResult<TopicDto>(topics);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetTopicById(int id,
            ITopicRepository topicRepository,
            IMapper mapper)
        {
            var topics = await topicRepository.GetTopicByIdAsync(id, true);
            return topics == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topics)));
        }

        private static async Task<IResult> GetTopicBySlug(string slug,
            ITopicRepository topicRepository,
            IMapper mapper)
        {
            var topics = await topicRepository.GetTopicBySlugAsync(slug, true);
            return topics == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có slug {slug}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topics)));
        }

        private static async Task<IResult> AddTopic(
            HttpContext context,
            IMapper mapper,
            ITopicRepository topicRepository,
            IMediaManager mediaManager)
        {
            var model = await TopicEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await topicRepository.IsTopicSlugExitedAsync(model.Id, slug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng"));
            }
            var topic = model.Id > 0 ? await topicRepository.GetTopicByIdAsync(model.Id) : null;
            if(topic == null)
            {
                topic = new Topic()
                {
                    RegistrationDate = DateTime.Now,
                };
            }
            topic.Title = model.Title;
            topic.DepartmentId = model.DepartmentId;
            topic.StatusId = model.StatusId;
            topic.Description = model.Description;
            topic.Note = model.Note;
            topic.EndDate = model.EndDate;
            topic.StudentNumbers = model.StudentNumbers;
            topic.Price = model.Price;
            topic.UrlSlug = model.Title.GenerateSlug();
            
            await topicRepository.AddOrUpdateTopicAsync(topic);
            return Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topic), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateTopic(
            int id,
            [AsParameters]TopicEditModel model,
            IMapper mapper,
            ITopicRepository topicRepository,
            IDepartmentRepository departmentRepository,
            ILecturerRepository lecturerRepository,
            IAppRepository appRepository,
            IMediaManager mediaManager)
        {
            var topic = await topicRepository.GetTopicByIdAsync(id);
            if(topic == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                    $"Không tìm thấy đề tài có id {id}"));
            }
            if(model.EndDate == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Thời gian nghiệm thu không được để trống"));
            }
            if (await departmentRepository.GetDepartmentByIdAsync(model.DepartmentId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy khoa có id = '{model.DepartmentId}' "));
            }
            if (await appRepository.GetStatusByIdAsync(model.StatusId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy trạng thái có id = '{model.StatusId}' "));
            }
            mapper.Map(model, topic);
            topic.Id = id;
            return await topicRepository.AddOrUpdateTopicAsync(topic)
               ? Results.Ok(ApiResponse.Success($"Thay đổi đề tài có id = {id} thành công"))
               : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có id = {id}"));
        }

        private static async Task<IResult> DeleteTopic(int id,
            ITopicRepository topicRepository)
        {
            return await topicRepository.RemoveTopicAsync(id)
                ? Results.Ok(ApiResponse.Success("Xóa đề tài thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có id = {id}"));
        }

        private static async Task<IResult> GetFilter(
            IDepartmentRepository departmentRepository,
            ILecturerRepository lecturerRepository,
            IAppRepository appRepository)
        {
            var model = new TopicFilterModel()
            {
                DepartmentList = (await departmentRepository.GetAllDepartmentAsync())
                .Select(d => new SelectListItem()
                {
                    Text = d.Name,
                    Value = d.Id.ToString(),
                }),
                LecturerList = (await lecturerRepository.GetLecturersAsync())
                .Select(l => new SelectListItem()
                {
                    Text = l.FullName,
                    Value = l.Id.ToString(),
                }),
                StatusList = (await appRepository.GetStatusAsync())
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }),
            };
            return Results.Ok(ApiResponse.Success(model));
        }

        private static async Task<IResult> RegisterTopic(
            int id,
            [AsParameters] TopicAddStudent model,
            IMapper mapper,
            ITopicRepository topicRepository,
            IStudentRepository studentRepository,
            IMediaManager mediaManager)
        {
            if (await topicRepository.GetTopicByIdAsync(id) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                        $"Không tìm thấy đề tài có id {id}"));
            }
            if (await studentRepository.GetStudentBySlugAsync(model.StudentSlug) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                        $"Không tìm thấy sinh viên có slug {model.StudentSlug}"));
            }
            return await topicRepository.RegisterTopic(id, model.StudentSlug)
                ? Results.Ok(ApiResponse.Success($"Đăng ký thành công"))
               : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có id {id}"));
        }

        private static async Task<IResult> SetTopicOutline(
            string slug,
            IFormFile outlineFile,
            ITopicRepository topicRepository,
            IMediaManager mediaManager)
        {
            var outlineUrl = await mediaManager.SaveFileAsync(
                outlineFile.OpenReadStream(),
                outlineFile.FileName, outlineFile.ContentType);
            if (string.IsNullOrWhiteSpace(outlineUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }
            await topicRepository.SetOutlineUrlAsync(slug, outlineUrl);
            return Results.Ok(ApiResponse.Success(outlineUrl));
        }

        private static async Task<IResult> SetTopicResult(
            string slug,
            IFormFile resultFile,
            ITopicRepository topicRepository,
            IMediaManager mediaManager)
        {
            var resultUrl = await mediaManager.SaveFileAsync(
                resultFile.OpenReadStream(),
                resultFile.FileName, resultFile.ContentType);
            if (string.IsNullOrWhiteSpace(resultUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }
            await topicRepository.SetResultUrlAsync(slug, resultUrl);
            return Results.Ok(ApiResponse.Success(resultFile));
        }

        private static async Task<IResult> SetTopicLecturer(
            HttpContext context,
            IMapper mapper,
            ITopicRepository topicRepository,
            IMediaManager mediaManager)
        {
            var model = await TopicAddLecturer.BindAsync(context);
            var topic = model.Id > 0 ? await topicRepository.GetTopicByIdAsync(model.Id) : null;
            topic.LecturerId = model.LecturerId;
            await topicRepository.AddOrUpdateTopicAsync(topic);
            return Results.Ok(ApiResponse.Success(mapper.Map<TopicItem>(topic), HttpStatusCode.Created));
        }

        private static async Task<IResult> IncreaseView(
            string slug,
            ITopicRepository topicRepository)
        {
            await topicRepository.IncreaseViewCountAsync(slug);
            return Results.Ok(ApiResponse.Success($"Đề tài có slug = {slug} đã tăng view thành công"));
        }
    }
}
