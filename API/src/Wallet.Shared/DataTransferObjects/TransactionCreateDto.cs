﻿using System.ComponentModel.DataAnnotations;

namespace Wallet.Shared.DataTransferObjects;

public class TransactionCreateDto {
    public DateTime Date { get; set; }
    [Required] public decimal Amount { get; set; }
    [MaxLength(100)] public string? Description { get; set; }
    [Required][MaxLength(50)] public string? Category { get; set; }
    [Required][MaxLength(20)] public string? Type { get; set; }
    [MaxLength(10)] public string? Currency { get; set; }
    [MaxLength(100)] public string? Location { get; set; }
    public string? Tags { get; set; }
    public string? Attachment { get; set; }
}