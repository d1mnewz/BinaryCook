﻿using System.Threading.Tasks;
using BinaryCook.Core.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BinaryCook.API.Infrastructure.Extensions
{
    public static class CommandResultExtensions
    {
        public static IActionResult ToCreatedAtResult(this ICommandResult result, string action = "GetById", string controller = null)
        {
            if (result.Succeeded)
                return new CreatedAtActionResult(action, controller, new { id = result.ReferenceId }, result.ReferenceId);
            return new BadRequestObjectResult(result.ValidationResult);
        }

        public static async Task<IActionResult> ToCreatedAtResult(this Task<ICommandResult> task, string action = "GetById", string controller = null)
            => (await task).ToCreatedAtResult(action, controller);

        public static IActionResult ToAcceptedAtResult(this ICommandResult result, string action = "GetById", string controller = null)
        {
            if (result.Succeeded)
                return new AcceptedAtActionResult(action, controller, new { id = result.ReferenceId }, result.ReferenceId);
            return new BadRequestObjectResult(result.ValidationResult);
        }

        public static async Task<IActionResult> ToAcceptedAtResult(this Task<ICommandResult> task, string action = "GetById", string controller = null)
            => (await task).ToAcceptedAtResult(action, controller);

        public static IActionResult ToOkResult(this ICommandResult result)
        {
            if (result.Succeeded)
                return new OkResult();
            return new BadRequestObjectResult(result.ValidationResult);
        }

        public static async Task<IActionResult> ToOkResult(this Task<ICommandResult> task) => (await task).ToOkResult();
    }
}