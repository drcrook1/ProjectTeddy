using Microsoft.Hadoop.Hive;
using ProjectTeddy.FSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTeddy.HadoopHandler.Entities
{
    public class WordRelationshipHiveRow : HiveRow
    {
        public string Id { get; set; }
        public string WordOne { get; set; }
        public int WordOneId { get; set; }
        public string WordTwo { get; set; }
        public int WordTwoId { get; set; }
        public float rScore { get; set; }

        //public FSWordRelationship convertToWordRelationship()
        //{
        //    return new FSWordRelationship(this.Id, this.WordOne, 
        //        this.WordOneId, this.WordTwo, this.WordTwoId, this.rScore);
        //}
    }
}
