using System.Collections.Generic;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// Find类型查找配置文件
    /// </summary>
    public class ACToolFindConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ACToolFindConfig() => controlDic = new Dictionary<string, List<Component>>();

        /// <summary>
        /// 关键词 例如V_
        /// </summary>

        public string KeyValue { get; set; }

        /// <summary>
        /// 选中的物体名称
        /// </summary>
        public string selectGoName { get; set; }

        /// <summary>
        /// 是否添加前缀
        /// </summary>
        public bool isAddPrefix { get; set; } = false;

        /// <summary>
        /// 是否赋值
        /// </summary>
        public bool isAssign { get; set; }

        /// <summary>
        /// 是否是GetSet生成
        /// </summary>
        public bool isGetSet { get; set; } = true;

        /// <summary>
        /// 用于代码生成输入的前缀
        /// </summary>
        public string beginStr { get; set; }

        /// <summary>
        /// 数据字典
        /// </summary>
        public Dictionary<string, List<Component>> controlDic { get; set; }
    }
}
