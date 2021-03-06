﻿/*Version: MPL 1.1/EUPL 1.1
 * 
 * The contents of this file are subject to the Mozilla Public License Version 
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at:
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 * 
 * The Original Code is Copyright The PEPPOL project (http://www.peppol.eu)
 * 
 * Alternatively, the contents of this file may be used under the
 * terms of the EUPL, Version 1.1 or - as soon they will be approved 
 * by the European Commission - subsequent versions of the EUPL 
 * (the "Licence"); You may not use this work except in compliance 
 * with the Licence.
 * You may obtain a copy of the Licence at:
 * http://www.osor.eu/eupl/european-union-public-licence-eupl-v.1.1
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the Licence is distributed on an "AS IS" basis,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Licence for the specific language governing permissions and
 * limitations under the Licence.
 * 
 * If you wish to allow use of your version of this file only
 * under the terms of the EUPL License and not to allow others to use
 * your version of this file under the MPL, indicate your decision by
 * deleting the provisions above and replace them with the notice and
 * other provisions required by the EUPL License. If you do not delete
 * thev provisions above, a recipient may use your version of this file
 * under either the MPL or the EUPL License.
 */

using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System;

namespace STARTLibrary.src.eu.peppol.start.security.configuration
{
    public class ValidationConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("rootCACertificateThumbprint", IsRequired = true)]
        public string RootCACertificateThumbprint
        {
            get { return (string)this["rootCACertificateThumbprint"]; }
            set { this["rootCACertificateThumbprint"] = value; }
        }

        [ConfigurationProperty("intermediateAcessPointCACertificateThumbprint", IsRequired = true)]
        public string IntermediateAcessPointCACertificateThumbprint
        {
            get { return (string)this["intermediateAcessPointCACertificateThumbprint"]; }
            set { this["intermediateAcessPointCACertificateThumbprint"] = value; }
        }

        [ConfigurationProperty("intermediateSmpCACertificateThumbprint", IsRequired = true)]
        public string IntermediateSmpCACertificateThumbprint
        {
            get { return (string)this["intermediateSmpCACertificateThumbprint"]; }
            set { this["intermediateSmpCACertificateThumbprint"] = value; }
        }
    }

    public class ValidationConfigurationElementCollection : ConfigurationElementCollection
    {
        public ValidationConfigurationElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ValidationConfigurationElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ValidationConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ValidationConfigurationElement)element).Name;
        }

        public ValidationConfigurationElement ByName(string name)
        {
            return this.Cast<ValidationConfigurationElement>().FirstOrDefault(
                validation => string.Equals(name, validation.Name)
                );
        }

        public string[] RootCAThumbprints { get { return Thumbprints(v => v.RootCACertificateThumbprint); } }
        public string[] AccessPointCAThumbprints { get { return Thumbprints(v => v.IntermediateAcessPointCACertificateThumbprint); } }
        public string[] SmpCAThumbprints { get { return Thumbprints(v => v.IntermediateSmpCACertificateThumbprint); } }

        public string[] RootCAThumbprintsByName(string elementName)
        {
            return RootCAThumbprintsByName(new[] { elementName });
        }

        public string[] AccessPointCAThumbprintsByName(string elementName)
        {
            return AccessPointCAThumbprintsByName(new[] { elementName });
        }

        public string[] SmpCAThumbprintsByName(string elementName)
        {
            return SmpCAThumbprintsByName(new[] { elementName });
        }

        public string[] RootCAThumbprintsByName(string[] elementNames)
        {
            return ThumbprintsByName(elementNames, v => v.RootCACertificateThumbprint);
        }

        public string[] AccessPointCAThumbprintsByName(string[] elementNames)
        {
            return ThumbprintsByName(elementNames, v => v.IntermediateAcessPointCACertificateThumbprint);
        }

        public string[] SmpCAThumbprintsByName(string[] elementNames)
        {
            return ThumbprintsByName(elementNames, v => v.IntermediateSmpCACertificateThumbprint);
        }

        private string[] Thumbprints(Func<ValidationConfigurationElement, string> selector)
        {
            return this.Cast<ValidationConfigurationElement>().Select(selector).ToArray();
        }

        private string[] ThumbprintsByName(string[] elementNames, Func<ValidationConfigurationElement, string> selector)
        {
            return this.Cast<ValidationConfigurationElement>().Where(

                    validation => elementNames.Contains(validation.Name)

                ).Select(selector).ToArray();
        }

    }
}
