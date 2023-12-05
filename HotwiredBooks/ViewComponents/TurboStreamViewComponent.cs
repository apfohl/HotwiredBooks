using HotwiredBooks.Models;
using Microsoft.AspNetCore.Mvc;
using MonadicBits;

namespace HotwiredBooks.ViewComponents;

public sealed record TurboStreamViewData(string Target, string Action, Maybe<string> Template);

public sealed class TurboStreamViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string target, TurboStreamAction action, Maybe<string> template) =>
        View(new TurboStreamViewData(
            target,
            action.ToString().ToLower(),
            template.Map(t => $"<template>{t}</template>")
        ));
}
