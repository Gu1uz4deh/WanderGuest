﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class Category : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
