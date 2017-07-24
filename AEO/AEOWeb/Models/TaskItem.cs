using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEOWeb.Models
{
    public class TaskItem
    {
        public int pID
        {
            get;
            set;
        }
        public string pName
        {
            get;
            set;
        }
        public string pStart
        {
            get;
            set;
        }
        public string pEnd
        {
            get;
            set;
        }
        public string pClass
        {
            get;
            set;
        }
        public string pLink
        {
            get;
            set;
        }
        public int pMile
        {
            get;
            set;
        }
        public string pRes
        {
            get;
            set;
        }
        public int pComp
        {
            get;
            set;
        }
        public int pGroup
        {
            get;
            set;
        }
        public int pParent
        {
            get;
            set;
        }
        public int pOpen
        {
            get;set;
        }
        public string pDepend
        {
            get;
            set;
        }
        public string pCaption
        {
            get;
            set;
        }
        public string pNotes
        {
            get;
            set;
        }
    }
}