﻿using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL
{
    public class DvdCopyTypeService
    {
        public DvdCopyType getByName(String name)
        {
            return new DvdCopyTypeDAO().getByName(name);         //Throws NoRecordException 
        }        
    }
}
