﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOTWM.Server {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BOTWM.Server.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &lt;?xml version=&quot;1.0&quot; ?&gt;
        ///&lt;root&gt;
        ///	&lt;animation&gt;
        ///		&lt;Name&gt;Float&lt;/Name&gt;
        ///		&lt;Hash&gt;140800401&lt;/Hash&gt;
        ///		&lt;Schedule&gt;1&lt;/Schedule&gt;
        ///		&lt;Animation&gt;1&lt;/Animation&gt;
        ///	&lt;/animation&gt;
        ///	&lt;animation&gt;
        ///		&lt;Name&gt;Bow_Attack_Motorcycle_Draw_Lower_B&lt;/Name&gt;
        ///		&lt;Hash&gt;-846693611&lt;/Hash&gt;
        ///		&lt;Schedule&gt;1&lt;/Schedule&gt;
        ///		&lt;Animation&gt;2&lt;/Animation&gt;
        ///	&lt;/animation&gt;
        ///	&lt;animation&gt;
        ///		&lt;Name&gt;Bow_Attack_Horse_Draw_Lower_B&lt;/Name&gt;
        ///		&lt;Hash&gt;238917901&lt;/Hash&gt;
        ///		&lt;Schedule&gt;1&lt;/Schedule&gt;
        ///		&lt;Animation&gt;3&lt;/Animation&gt;
        ///	&lt;/animation&gt;
        ///	&lt;animation&gt;
        ///		&lt;Name&gt;Bow_Attack_ [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string animationHashes {
            get {
                return ResourceManager.GetString("animationHashes", resourceCulture);
            }
        }
    }
}
