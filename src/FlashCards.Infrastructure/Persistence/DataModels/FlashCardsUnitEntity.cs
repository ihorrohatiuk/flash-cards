﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlashCards.Core.Domain.Entities;

namespace FlashCards.Infrastructure.Persistence.DataModels;

public class FlashCardsUnitEntity : FlashCardsUnit
{
    public UserEntity User { get; set; }
}
