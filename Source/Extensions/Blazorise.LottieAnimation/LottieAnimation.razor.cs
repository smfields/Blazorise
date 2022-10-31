﻿#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace Blazorise.LottieAnimation;

/// <summary>
/// The LottieAnimation component embeds JSON-based Lottie animations into the document.
/// </summary>
public partial class LottieAnimation : BaseComponent, IAsyncDisposable
{
    #region Members
    
    #endregion
    
    #region Methods

    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
    {
        if ( JSModule == null )
        {
            DotNetObjectRef ??= DotNetObjectReference.Create( this );

            JSModule = new JSLottieAnimationModule( JSRuntime, VersionProvider );
        }

        return base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnFirstAfterRenderAsync()
    {
        JSAnimationReference = await JSModule.Initialize( DotNetObjectRef, ElementRef, ElementId, new
        {
            Path
        } );
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsync( bool disposing )
    {
        if ( disposing && Rendered )
        {
            await JSModule.SafeDestroy( ElementRef, ElementId );

            await JSModule.SafeDisposeAsync();

            if ( DotNetObjectRef != null )
            {
                DotNetObjectRef.Dispose();
                DotNetObjectRef = null;
            }

            if ( JSAnimationReference != null )
            {
                await JSAnimationReference.DisposeAsync();
                JSAnimationReference = null;
            }
        }

        await base.DisposeAsync( disposing );
    }
    
    #endregion

    #region Parameters

    /// <inheritdoc/>
    protected override bool ShouldAutoGenerateId => true;

    /// <summary>
    /// Reference to the object that should be accessed through JSInterop.
    /// </summary>
    protected DotNetObjectReference<LottieAnimation> DotNetObjectRef { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="JSLottieAnimationModule"/> instance.
    /// </summary>
    protected JSLottieAnimationModule JSModule { get; private set; }
    
    /// <summary>
    /// Gets or sets the reference to the JS lottie animation object
    /// </summary>
    protected IJSObjectReference JSAnimationReference { get; private set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Inject]
    private IVersionProvider VersionProvider { get; set; }
    
    [Parameter] public string Path { get; set; }
    
    /// <summary>
    /// Sent when animation playback enters a new frame
    /// </summary>
    [Parameter] public Func<EnteredFrameEventArgs, Task> EnteredFrame { get; set; }
    
    /// <summary>
    /// Specifies the content to be rendered inside this <see cref="LottieAnimation"/>.
    /// </summary>
    [Parameter] public RenderFragment ChildContent { get; set; }

    #endregion
}