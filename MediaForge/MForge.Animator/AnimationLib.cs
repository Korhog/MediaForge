using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MForge.Animator.Animations;
using System.Drawing;
using Windows.UI.Xaml;
using MForge.Animator.BeginAnimations;

namespace MForge.Animator
{
    /// <summary>
    /// Библиотека анимаций
    /// </summary>
    public class AnimationLib
    {
        public class AnimationDesc
        {
            public string Name { get; set; }
            public Type Type { get; set; }
        }

        public static List<AnimationDesc> GetAnimations()
        {
            var assembly = Assembly.GetAssembly(typeof(AnimationLib));

            return assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IAnimation)))
                .Select(x => new AnimationDesc { Name = x.Name, Type = x })
                .ToList();
        }

        public static IAnimation Create<T> () where T : IAnimation
        {
            return Activator.CreateInstance<T>();
        }
    }
}
