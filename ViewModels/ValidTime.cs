﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GigHub.ViewModels
{
    public class ValidTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //First we make sure its a valid date. Tryparseexact helps us to define format

            DateTime dateTime;
            var isValid = DateTime.TryParseExact(Convert.ToString(value),//Did not call value.ToString because Value can be NULL
                "HH:mm",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime);
            //isValid is boolean. 
            return (isValid);
        }
    }
}