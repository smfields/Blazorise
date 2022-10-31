﻿#region Using directives

using System.Threading.Tasks;
using Blazorise.Modules;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace Blazorise.LottieAnimation;

/// <summary>
/// Default implementation of the lottie JS module.
/// </summary>
public class JSLottieAnimationModule : BaseJSModule, IJSDestroyableModule
{
    #region Constructors

    /// <summary>
    /// Default module constructor.
    /// </summary>
    /// <param name="jsRuntime">JavaScript runtime instance.</param>
    /// <param name="versionProvider">Version provider.</param>
    public JSLottieAnimationModule( IJSRuntime jsRuntime, IVersionProvider versionProvider ) : base( jsRuntime, versionProvider )
    {
    }

    #endregion

    #region Methods

    public virtual async ValueTask Initialize( DotNetObjectReference<LottieAnimation> dotNetObjectReference, ElementReference elementRef, string elementId, object options )
    {
        var moduleInstance = await Module;

        await moduleInstance.InvokeVoidAsync( "initialize", dotNetObjectReference, elementRef, elementId, options );
    }
    
    public virtual async ValueTask Destroy( ElementReference elementRef, string elementId )
    {
        var moduleInstance = await Module;

        await moduleInstance.InvokeVoidAsync( "destroy", elementRef, elementId );
    }

    #endregion

    #region Properties

    /// <inheritdoc/>
    public override string ModuleFileName => $"./_content/Blazorise.LottieAnimation/lottie-animation.js?v={VersionProvider.Version}";

    #endregion
}