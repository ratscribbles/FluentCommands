using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;

namespace FluentCommands
{
    public class TestingContext : CommandContext<TestingContext>
    {
        protected override void OnBuilding(ModuleBuilder m)
        {
            m["hi"]
                .HasAliases("oweoweow", "djidaijsd");
            m["poggers"]
                .HasHelpDescription("UEUEUEUE");
        }
    }
}
