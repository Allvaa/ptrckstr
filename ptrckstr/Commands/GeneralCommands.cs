﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Threading.Tasks;

namespace ptrckstr.Commands
{
    class GeneralCommands
    {
        [Command("ping")]
        [Description("WebSocket ping.")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.RespondAsync($"Pong! `{ctx.Client.Ping}ms`");
        }

        [Command("say")]
        [Description("Say what you say.")]
        [Aliases("echo")]
        public async Task Say(CommandContext ctx)
        {
            await ctx.RespondAsync(ctx.RawArgumentString
                .Replace("@everyone", "@\u200Beveryone")
                .Replace("@here", "@\u200Bhere")
                );
        }

        [Command("avatar")]
        [Description("User's profile picture")]
        [Aliases("pfp", "ava")]
        public async Task Avatar(CommandContext ctx, [RemainingText]DiscordMember Member)
        {
            if (Member == null) Member = ctx.Member;
            DiscordEmbed Embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Blurple)
                .WithDescription($"[Avatar URL]({Member.AvatarUrl})")
                .WithImageUrl(Member.AvatarUrl)
                .Build();
            await ctx.RespondAsync(embed: Embed);
        }

        [Command("userinfo")]
        [Description("User's information.")]
        [Aliases("uinfo", "ui")]
        public async Task UserInfo(CommandContext ctx, [RemainingText]DiscordMember Member)
        {
            if (Member == null) Member = ctx.Member;
            DiscordEmbed Embed = new DiscordEmbedBuilder()
                .WithAuthor($"{Member.Username}'s Information", null, Member.AvatarUrl)
                .WithColor(DiscordColor.Blurple)
                .WithDescription($@"
• ID: {Member.Id}
• Tag: {Member.Username}#{Member.Discriminator} | {Member.Mention}
• Type: {(Member.IsBot ? "Bot" : "Human")}
• Created At: {Member.CreationTimestamp.UtcDateTime}
• Joined At: {Member.JoinedAt.UtcDateTime}
• Presence: {(Member.Presence.Game != null ? Member.Presence.Game.Name : "*Nothing Played")} ({Member.Presence.Status})
")
                .Build();
            await ctx.RespondAsync(embed: Embed);
        }

        [Command("serverinfo")]
        [Description("Current Guild's information")]
        [Aliases("guildinfo", "ginfo", "si")]
        public async Task ServerInfo(CommandContext ctx)
        {
            DiscordGuild Guild = ctx.Guild;
            DiscordEmbed Embed = new DiscordEmbedBuilder()
                .WithAuthor(Guild.Name, null, Guild.IconUrl)
                .WithColor(DiscordColor.Blurple)
                .WithDescription($@"
• ID {Guild.Id}
• Owner: {Guild.Owner.Mention}
• Region: {Guild.RegionId}
• Verification Level: {Guild.VerificationLevel}
• Member Count: {Guild.MemberCount}
• Channel Count: {Guild.Channels.Count}
• Created At: {Guild.CreationTimestamp.UtcDateTime}
")
                .Build();
            await ctx.RespondAsync(embed: Embed);
        }

        [Command("eval")]
        [RequireOwner]
        public async Task Eval(CommandContext ctx, string code)
        {
            if (code == null)
            {
                return;
            }

            var options = ScriptOptions.Default.AddImports("System.Collections.Generic");
            var globals = new ScriptGlobals { ctx = ctx, client = ctx.Client };

            try
            {
                var start = DateTime.Now.Millisecond;
                var script = CSharpScript.Create(code, options, typeof(ScriptGlobals));
                var scriptState = await script.RunAsync(globals);
                var returnValue = scriptState.ReturnValue;
                if (returnValue != null)
                    await ctx.RespondAsync($"<:yes:615730280619048970> **Success :**\n" +
                        "```\n" +
                        returnValue.ToString() +
                        "\n```");
            }
            catch (Exception ex)
            {
                await ctx.RespondAsync("<:no:615730302936940554> **Error :**\n" +
                    "```ini\n" +
                    ex.Message +
                    "\n```");
            }
        }
    }

    public class ScriptGlobals
    {
        public CommandContext ctx { get; internal set; }
        public DiscordClient client { get; internal set; }
    }
}
