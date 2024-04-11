using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iso20022T4Templates
{
    public static class T4Interfacer
    {
        public static void Accessor()
        {
            var template = new ClassTemplates.ClassTemplate
            {
            };
            var content = template.TransformText();
        }
    }
}
