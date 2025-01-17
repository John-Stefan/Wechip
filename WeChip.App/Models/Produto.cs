﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChip.App.Models
{
    public class Produto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("preco")]
        public decimal Preco { get; set; }

        [JsonProperty("tipoid")]
        public int TipoId { get; set; }

        [JsonProperty("tipo")]
        public int Tipo { get; set; }

        [DisplayName("Tipo do produto")]
        [JsonIgnore]        
        public string TipoDescricao { get; set; }
    }
}
