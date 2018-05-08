﻿using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BinaryCook.API.Infrastructure
{
    public class ProducesResponseCodeTypeAttribute : ProducesResponseTypeAttribute
    {
        public ProducesResponseCodeTypeAttribute(HttpStatusCode statusCode) : base((int) statusCode)
        {
        }

        public ProducesResponseCodeTypeAttribute(Type type, HttpStatusCode statusCode) : base(type, (int) statusCode)
        {
        }
    }
}