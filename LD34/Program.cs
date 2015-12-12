using Cog;
using Cog.Modules.EventHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD34
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Initialize<XnaRenderer.XnaRenderer, IrrKlangModule.IrrKlangAudioModule>();

            Engine.DesiredResolution = new Vector2(1280f, 720f);

            Engine.EventHost.RegisterEvent<InitializeEvent>(0, ev =>
            {
                Engine.ResourceHost.LoadDictionary("main", "Resources");
                Engine.SceneHost.Push(Engine.SceneHost.CreateLocal<MainScene>());
            });

            Engine.StartGame("Ludum Dare 34", Cog.Modules.Renderer.WindowStyle.Default);
        }
    }
}
