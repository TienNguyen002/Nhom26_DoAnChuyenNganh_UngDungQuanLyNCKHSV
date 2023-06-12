﻿using FluentValidation;
using WebApi.Models.Lecturer.Account;

namespace WebApi.Validations.Lecturers
{
    public class LecturerAccountValidator : AbstractValidator<LecturerCreateAccount>
    {
        public LecturerAccountValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .MaximumLength(1000)
                .WithMessage("Email chỉ tối đa 1000 ký tự");

            RuleFor(l => l.Password)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống")
                .MaximumLength(1000)
                .WithMessage("Mật khẩu chỉ tối đa 1000 ký tự");

            RuleFor(l => l.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Mật khẩu xác nhận không được để trống")
                .MaximumLength(1000)
                .WithMessage("Mật khẩu xác nhận chỉ tối đa 1000 ký tự");

            RuleFor(l => l.FullName)
                .NotEmpty()
                .WithMessage("Họ và tên không được để trống")
                .MaximumLength(1000)
                .WithMessage("Họ và tên chỉ tối đa 1000 ký tự");
        }
    }
}
