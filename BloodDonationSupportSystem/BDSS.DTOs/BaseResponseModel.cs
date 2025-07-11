﻿namespace BDSS.DTOs;

public class BaseResponseModel<T>
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}
