﻿using ArbitraryCollectionMgmt.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.ViewModels
{
    public class ViewItemVM
    {
        public ItemCustomAttributeValueDTO Item;
        public List<ItemTagDTO> ItemTags;
        public CollectionDTO Collection;
    }
}
