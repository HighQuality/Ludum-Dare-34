using Cog.Modules.Renderer;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cog.Modules.EventHost;
using Cog;
using System.Diagnostics;

namespace LD34
{
    [Resource(ContainerName = "main", Filename = "ship.png", Key = "ship")]
    [Resource(ContainerName = "main", Filename = "smoke.png", Key = "smoke")]
    class Player : PhysicsObject
    {
        public List<Particle> list = new List<Particle>();
        public Stopwatch watch = Stopwatch.StartNew();

        public Texture SmokeTexture;

        public float ThrustersForce = 1000f;
        public bool ThrustersAreOn;

        float smokeParticleAccumulator,
            smokeParticleSpawnInterval = 0.015f;

        public bool IsOnGround;

        public bool IsDead;

        public Player()
            : base("ship")
        {
            SmokeTexture = Resources.GetTexture("smoke");

            RegisterEvent<ButtonDownEvent>(0, (int)Mouse.Button.Left, ButtonDown);
            RegisterEvent<DrawEvent>(-100, Draw);
        }

        public void Draw(DrawEvent ev)
        {
            float deltaTime = (float)watch.Elapsed.TotalSeconds;
            watch.Restart();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UpdateAndDraw(0f, ev.RenderTarget) == false)
                {
                    Particle.Recycle(list[i]);
                    list[i] = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                    i--;
                }
            }

            using (var context = Engine.Renderer.AdditiveBlend.Activate())
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].UpdateAndDraw(deltaTime, ev.RenderTarget) == false)
                    {
                        Particle.Recycle(list[i]);
                        list[i] = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        i--;
                    }
                }
            }
        }

        public void ButtonDown(ButtonDownEvent ev)
        {
            ThrustersAreOn = true;
            ev.ButtonUpCallback += () =>
            {
                ThrustersAreOn = false;
            };
        }

        public void SmokeParticle(Vector2 relativePosition)
        {
            relativePosition = relativePosition.Rotate(LocalRotation);
            Vector2 beginSpeed = (LocalRotation - Angle.FromDegree(180f)).Unit * 600f + Velocity;
            Color color = new Color(241, 196, 50, 255);
            list.Add(Particle.Create(SmokeTexture, LocalCoord + relativePosition, beginSpeed, Vector2.Zero, Vector2.One * 0.5f, Vector2.One * 3f, 0f, 0f, color * 0.6f, new Color(32, 32, 32, -64), 0.25f));
        }

        public override void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            if (IsDead == false)
            {
                if (IsOnGround == false || ThrustersAreOn)
                {
                    if (WorldCoord.Y > -16f)
                    {
                        if (LocalRotation.Degree < 360f - 45f && LocalRotation.Degree > 360f - 135f && Velocity.Length < 200f)
                        {
                            LocalRotation = Angle.FromDegree(-90f);
                            WorldCoord = new Vector2(WorldCoord.X, -16f);
                            Velocity = Vector2.Zero;
                            PositionAccumulator = Vector2.Zero;
                            IsOnGround = true;
                        }
                        else
                        {
                            // Player died
                            IsDead = true;
                            Sprite.Remove();
                        }
                    }
                    else
                    {
                        IsOnGround = false;
                    }

                    if (IsOnGround == false)
                    {
                        Vector2 mousePosition = Mouse.Location + Scene.Camera.WorldCoord - Engine.Resolution / 2f;
                        LocalRotation = (mousePosition - WorldCoord).Angle;

                        if (ThrustersAreOn)
                        {
                            Velocity += LocalRotation.Unit * ThrustersForce * ev.DeltaTime;

                            smokeParticleAccumulator += ev.DeltaTime;

                            while (smokeParticleAccumulator >= smokeParticleSpawnInterval)
                            {
                                SmokeParticle(new Vector2(2f - 16f, 8f - 16f));
                                SmokeParticle(new Vector2(2f - 16f, 23f - 16f));
                                smokeParticleAccumulator -= smokeParticleSpawnInterval;
                            }
                        }

                        base.PhysicsUpdate(ev);
                    }
                }
            }
        }
    }
}
