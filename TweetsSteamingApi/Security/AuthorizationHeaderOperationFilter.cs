using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TweetsSteamingApi.Security
{
    /// <summary>
    ///     This is an Operation Filter class used to add Authorization/apiKey header parameters
    /// </summary>
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {

        /// <summary>
        ///     <see cref="IOperationFilter.Apply(OpenApiOperation, OperationFilterContext)"/>
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // If current method is not anonymous athen adding authorization parameter in header
            if (IsUsingAuth(context))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Bearer JWT Token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }

        /// <summary>
        ///		Checks to see if a operation filter context is for an operation that is using auth
        /// </summary>
        /// <param name="context">
        ///		Operation filter context to check for
        /// </param>
        /// <returns>
        ///		True if the operation filter context is for an operation with auth.
        ///		False if not using auth.
        /// </returns>
        private static bool IsUsingAuth(OperationFilterContext context)
        {
            // Check if the controller has an Authorize attribute
            var isAuthorized =
                context
                    .MethodInfo
                    .DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any() ||
                context
                    .MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any();

            // Setting the flag true or false based on if current method is anonymous or not
            var allowAnonymous =
                context
                    .MethodInfo
                    .CustomAttributes
                    .Select(filter => filter.AttributeType)
                    .Any(filter => filter.FullName == typeof(AllowAnonymousAttribute).FullName);

            // If current method is not anonymous athen adding authorization parameter in header
            return isAuthorized && !allowAnonymous;
        }
    }
}
