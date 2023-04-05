﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PumpClient.PumpServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StatisticsService", Namespace="http://schemas.datacontract.org/2004/07/PumpService")]
    [System.SerializableAttribute()]
    public partial class StatisticsService : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AllTactsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ErrorTactsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SucceesTactsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AllTacts {
            get {
                return this.AllTactsField;
            }
            set {
                if ((this.AllTactsField.Equals(value) != true)) {
                    this.AllTactsField = value;
                    this.RaisePropertyChanged("AllTacts");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ErrorTacts {
            get {
                return this.ErrorTactsField;
            }
            set {
                if ((this.ErrorTactsField.Equals(value) != true)) {
                    this.ErrorTactsField = value;
                    this.RaisePropertyChanged("ErrorTacts");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SucceesTacts {
            get {
                return this.SucceesTactsField;
            }
            set {
                if ((this.SucceesTactsField.Equals(value) != true)) {
                    this.SucceesTactsField = value;
                    this.RaisePropertyChanged("SucceesTacts");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Miscrosoft.ServiceModel.Samples", ConfigurationName="PumpServiceReference.IPumpService", CallbackContract=typeof(PumpClient.PumpServiceReference.IPumpServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IPumpService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Miscrosoft.ServiceModel.Samples/IPumpService/RunSript", ReplyAction="http://Miscrosoft.ServiceModel.Samples/IPumpService/RunSriptResponse")]
        void RunSript();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Miscrosoft.ServiceModel.Samples/IPumpService/RunSript", ReplyAction="http://Miscrosoft.ServiceModel.Samples/IPumpService/RunSriptResponse")]
        System.Threading.Tasks.Task RunSriptAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Miscrosoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScript", ReplyAction="http://Miscrosoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScriptRespons" +
            "e")]
        void UpdateAndCompileScript(string filename);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Miscrosoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScript", ReplyAction="http://Miscrosoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScriptRespons" +
            "e")]
        System.Threading.Tasks.Task UpdateAndCompileScriptAsync(string filename);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPumpServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Miscrosoft.ServiceModel.Samples/IPumpService/UpdateStatistics", ReplyAction="http://Miscrosoft.ServiceModel.Samples/IPumpService/UpdateStatisticsResponse")]
        void UpdateStatistics(PumpClient.PumpServiceReference.StatisticsService statistics);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPumpServiceChannel : PumpClient.PumpServiceReference.IPumpService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PumpServiceClient : System.ServiceModel.DuplexClientBase<PumpClient.PumpServiceReference.IPumpService>, PumpClient.PumpServiceReference.IPumpService {
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void RunSript() {
            base.Channel.RunSript();
        }
        
        public System.Threading.Tasks.Task RunSriptAsync() {
            return base.Channel.RunSriptAsync();
        }
        
        public void UpdateAndCompileScript(string filename) {
            base.Channel.UpdateAndCompileScript(filename);
        }
        
        public System.Threading.Tasks.Task UpdateAndCompileScriptAsync(string filename) {
            return base.Channel.UpdateAndCompileScriptAsync(filename);
        }
    }
}
