namespace LightADO {
    public class Types {
        /// <summary>
        /// The Configuration Sections
        /// </summary>
        internal enum ConfigurationSections {
            /// <summary>
            /// Connection String Section
            /// </summary>
            ConnectionString,

            /// <summary>
            /// The App Settings section
            /// </summary>
            AppSettings
        }

        /// <summary>
        /// The direction of setting 
        /// up the default value.
        /// </summary>
        public enum Directions {
            /// <summary>
            /// Only with Query.
            /// </summary>
            WithQuery,

            /// <summary>
            /// Only with non Query.
            /// </summary>
            WithNonQuery,

            /// <summary>
            /// With both of them.
            /// </summary>
            WithBoth
        }

        /// <summary>
        /// The type of the passed value.
        /// </summary>
        public enum ValueTypes {
            /// <summary>
            /// Get the value from the Object Property.
            /// </summary>
            Properties,

            /// <summary>
            /// Get The type from the object methods.
            /// </summary>
            Methods,

            /// <summary>
            /// straightforward value
            /// </summary>
            Value
        }

        /// <summary>
        /// Encrypt Engine Options
        /// </summary>
        internal enum OprationType {
            /// <summary>
            /// Encrypt string.
            /// </summary>
            Encrypt,

            /// <summary>
            /// decrypt string.
            /// </summary>
            Descrypt
        }

        /// <summary>
        /// Type of supported Format.
        /// </summary>
        public enum FormatType {
            /// <summary>
            /// As Xml
            /// </summary>
            XML,

            /// <summary>
            /// As JSON
            /// </summary>
            Json,
        }
    }
}