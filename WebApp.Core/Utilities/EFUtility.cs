using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace WebApp.Core.Utilities
{
    public class EFUtility
    {
        public static string GetAllValidationErrorMessage(DbEntityValidationException ex)
        {
            var errorMessages = ex.EntityValidationErrors
                .SelectMany(x => x.ValidationErrors)
                .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);
            var errorMessage = string.Concat(ex.Message, ". Các Lỗi Kiểm tra dữ liệu: ", fullErrorMessage);
            return errorMessage;
        }
    }
}
