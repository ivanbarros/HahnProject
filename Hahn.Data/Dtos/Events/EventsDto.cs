﻿using Hahn.Domain.Entities.BaseEntity;

namespace Hahn.Data.Dtos.Events;

public class EventsDto : CommonEntity
{
    public string Local { get; set; }
    public string DataEvento { get; set; }
    public string Tema { get; set; }
    public int QtdPessoas { get; set; }
    public string Lote { get; set; }
    public string ImagemUrl { get; set; }
}
