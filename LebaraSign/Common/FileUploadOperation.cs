using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace LebaraSign.Common;
public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if(operation.OperationId == "UploadFile")
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            },
                            Required = new HashSet<string>{"file"}
                        }
                    }
                }
            };
        }

        //var fileUploadParams = context.MethodInfo.GetParameters()
        //    .Where(p => p.ParameterType == typeof(IFormFile));

        //if (!fileUploadParams.Any())
        //{
        //    return;
        //}

        //operation.Parameters.Clear();

        //foreach (var param in fileUploadParams)
        //{
        //    operation.Parameters.Add(new OpenApiParameter
        //    {
        //        Name = param.Name,
        //        In = ParameterLocation.Header,
        //        Schema = new OpenApiSchema
        //        {
        //            Type = "string",
        //            Format = "binary"
        //        },
        //        Required = true
        //    });
        //}
    }
}
