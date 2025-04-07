﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BuisinessLogicLayer.Commands
{
    public class DeleteProductCommand:IRequest<bool>
    {
        public int ProductId {  get; set; }
    }
}
