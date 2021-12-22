using Autofac;
using TagsCloudContainerCore.Result;

namespace WinCloudLayouterConsoleUI.DI;

// ReSharper disable once InconsistentNaming
public static class DIExtension
{
    public static Result<IContainer> TryResolve<TResolve>(this Result<IContainer> containerRes,
        out TResolve resolved) where TResolve : notnull
    {
        if (!containerRes.IsSuccess)
        {
            resolved = default!;
            return ResultExtension.Fail<IContainer>(containerRes.Error);
        }

        try
        {
            resolved = containerRes.GetValueOrThrow().Resolve<TResolve>();
            return containerRes;
        }
        catch (Exception e)
        {
            resolved = default!;
            return ResultExtension.Fail<IContainer>($"{e.GetType().Name} {e.Message}");
        }
    }
}