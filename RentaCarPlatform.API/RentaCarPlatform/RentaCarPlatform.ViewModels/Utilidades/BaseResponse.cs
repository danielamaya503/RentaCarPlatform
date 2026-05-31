using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RentaCarPlatform.ViewModels.Utilidades
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }

        // ─── Respuestas exitosas ───────────────────────────────────────

        /// <summary>200 OK — Consulta o actualización exitosa.</summary>
        public static BaseResponse<T> Ok(T data, string message = "") =>
            new()
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = (int)HttpStatusCode.OK
            };

        /// <summary>201 Created — Recurso creado exitosamente.</summary>
        public static BaseResponse<T> Created(T data, string message = "") =>
            new()
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = (int)HttpStatusCode.Created
            };

        // ─── Respuestas de error ───────────────────────────────────────

        /// <summary>400 Bad Request — Datos inválidos o regla de negocio violada.</summary>
        public static BaseResponse<T> BadRequest(string message, List<string>? errors = null) =>
            new()
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Errors = errors
            };

        /// <summary>401 Unauthorized — No autenticado.</summary>
        public static BaseResponse<T> Unauthorized(string message = "No autenticado.") =>
            new()
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = (int)HttpStatusCode.Unauthorized
            };

        /// <summary>403 Forbidden — Sin permisos suficientes.</summary>
        public static BaseResponse<T> Forbidden(string message = "No tienes permisos para realizar esta acción.") =>
            new()
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = (int)HttpStatusCode.Forbidden
            };

        /// <summary>404 Not Found — Recurso no encontrado.</summary>
        public static BaseResponse<T> NotFound(string message = "Recurso no encontrado.") =>
            new()
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = (int)HttpStatusCode.NotFound
            };

        /// <summary>409 Conflict — Conflicto de datos (duplicados, restricciones).</summary>
        public static BaseResponse<T> Conflict(string message) =>
            new()
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = (int)HttpStatusCode.Conflict
            };

        /// <summary>500 Internal Server Error — Error inesperado del servidor.</summary>
        public static BaseResponse<T> Fail(string message, List<string>? errors = null) =>
            new()
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Errors = errors
            };
    }
}
