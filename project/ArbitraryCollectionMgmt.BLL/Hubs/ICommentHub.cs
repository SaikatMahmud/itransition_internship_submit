using ArbitraryCollectionMgmt.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Hubs
{
    public interface ICommentHub
    {
        Task ReceiveComment(CommentDTO comment);
    }
}
