﻿using Carter;
using Core.Collections;
using Core.DTO.Topic;
using Core.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Services.Apps.Departments;
using Services.Apps.Lecturers;
using Services.Apps.Others;
using Services.Apps.Topics;
using Services.Media;
using System.Net;
using WebApi.Filters;
using WebApi.Models;
using WebApi.Models.Student;
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

            routeGroupBuilder.MapGet("/{id:int}", GetTopicById)
                  .WithName("GetTopicById")
                  .Produces<ApiResponse<TopicDto>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetTopicBySlug)
                  .WithName("GetTopicBySlug")
                  .Produces<ApiResponse<TopicDto>>();

            routeGroupBuilder.MapPost("/", AddTopic)
                .WithName("AddCourse")
                .AddEndpointFilter<ValidatorFilter<TopicEditModel>>()
                .Produces<ApiResponse<TopicDto>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTopic)
                .WithName("UpdateCourse")
                .AddEndpointFilter<ValidatorFilter<TopicEditModel>>()
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTopic)
                .WithName("DeleteTopic")
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
                .WithName("GetTopicFilter")
                .Produces<ApiResponse<TopicFilterModel>>();
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
            [AsParameters] TopicEditModel model,
            IMapper mapper,
            ITopicRepository topicRepository,
            IDepartmentRepository departmentRepository,
            ILecturerRepository lecturerRepository,
            IAppRepository appRepository,
            IMediaManager mediaManager)
        {
            if(await departmentRepository.GetDepartmentByIdAsync(model.DepartmentId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy khoa có id = '{model.DepartmentId}' "));
            }
            if (await appRepository.GetStatusByIdAsync(model.StatusId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy trạng thái có id = '{model.StatusId}' "));
            }
            var topic = mapper.Map<Topic>(model);
            topic.RegistrationDate = DateTime.Now;
            if(await topicRepository.IsTopicSlugExitedAsync(0, topic.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"UrlSlug '{topic.UrlSlug}' đã được sử dụng"));
            }
            await topicRepository.AddOrUpdateTopicAsync(topic);
            return Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topic), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateTopic(
            int id,
            TopicEditModel model,
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
                ProcessList = (await appRepository.GetProcessAsync())
                .Select(p =>  new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                })
            };
            return Results.Ok(ApiResponse.Success(model));
        }
    }
}
