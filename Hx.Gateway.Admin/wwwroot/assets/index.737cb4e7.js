import{_ as R,u as K,i as w}from"./index.48b299d9.js";/* empty css              */import{u as T}from"./loading.3f202aeb.js";/* empty css              */import"./common.546a3b76.js";/* empty css               *//* empty css               *//* empty css               */import{d as j,e as k,o as E,aG as A,A as c,B as N,aH as e,aF as l,aJ as r,aK as u,aC as b,aD as g,E as W,u as F,aS as L,bC as G,b4 as M,bx as Q,a_ as J,bH as z,bD as X,bI as Y,bJ as Z,bK as x,bL as ee,b2 as le,b1 as ae,b3 as oe}from"./arco.1db1f704.js";import{g as te}from"./project.6ad6264a.js";import{c as de,b as re,e as ie,f as se}from"./global-configuration.6f352e3d.js";import{u as ue,j as ne}from"./vue.bbd05113.js";import"./chart.520624f6.js";import"./ocelot-options.514ee915.js";const pe={class:"container"},me=r("http"),ve=r("https"),ce=r("1.0"),fe=r("1.1"),be=r("2.0"),ge=r("http"),ye=r("https"),Ve=r("Consul"),he=r("PollConsul"),Oe=r("Eureka"),$e=r("ms"),_e=r("ms"),ke={class:"actions"},De=j({setup(Pe){const q=ue(),H=ne(),{t:D}=K(),o=k(de()),$=k(),_=k([]),{loading:C,setLoading:P}=T(),B=async()=>{$.value&&$.value.validate(async a=>{if(a==null){P(!0);var t=!1;if(w.exports.isEmpty(o.value.id)){const{data:O}=await re(o.value);t=O}else{const{data:O}=await ie(o.value);t=O}t?(L.success({content:D("submit.success"),duration:5*1e3}),q.push({name:"global-search"})):L.error({content:D("submit.fail"),duration:5*1e3}),P(!1)}})};return(async()=>{try{const a=H.query.id;if(!w.exports.isEmpty(a)){const{data:t}=await se(a);o.value=t}}catch(a){console.log(a)}})(),E(async()=>{const{data:a}=await te();a&&(_.value=a,console.log(_.value,"projectOptions"))}),(a,t)=>{const O=A("Breadcrumb"),h=G,i=M,s=Q,f=J,m=z,y=X,n=Y,v=Z,p=x,V=ee,U=le,S=ae,I=oe;return c(),N("div",pe,[e(O,{items:["menu.globalconfiguration","menu.globalconfiguration.create"]},null,8,["items"]),e(I,{ref_key:"formRef",ref:$,model:o.value},{default:l(()=>[e(U,{direction:"vertical",size:16},{default:l(()=>[e(y,{class:"general-card"},{title:l(()=>[r(u(a.$t("gc.basic.title")),1)]),default:l(()=>[e(m,{gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{field:"projectId",label:a.$t("project.label.name"),rules:[{required:!0,message:a.$t("project.form.name.required.errmsg")}]},{default:l(()=>[e(h,{modelValue:o.value.projectId,"onUpdate:modelValue":t[0]||(t[0]=d=>o.value.projectId=d),"allow-clear":"",options:_.value,"field-names":{value:"id",label:"name"},placeholder:a.$t("select.options.default")},null,8,["modelValue","options","placeholder"])]),_:1},8,["label","rules"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.basic.name.label"),field:"name",rules:[{required:!0,message:a.$t("gc.form.name.required.errmsg")}]},{default:l(()=>[e(f,{modelValue:o.value.name,"onUpdate:modelValue":t[1]||(t[1]=d=>o.value.name=d),placeholder:a.$t("gc.basic.name.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label","rules"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.basic.baseUrl.label"),field:"baseUrl"},{default:l(()=>[e(f,{modelValue:o.value.baseUrl,"onUpdate:modelValue":t[2]||(t[2]=d=>o.value.baseUrl=d),placeholder:a.$t("gc.basic.baseUrl.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.basic.requestIdKey.label"),field:"requestIdKey",rules:[{required:!0,message:a.$t("gc.form.requestIdKey.required.errmsg")}]},{default:l(()=>[e(f,{modelValue:o.value.requestIdKey,"onUpdate:modelValue":t[3]||(t[3]=d=>o.value.requestIdKey=d),placeholder:a.$t("gc.basic.requestIdKey.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label","rules"])]),_:1})]),_:1})]),_:1}),e(y,{class:"general-card"},{title:l(()=>[r(u(a.$t("gc.downstream.title")),1)]),default:l(()=>[e(m,{gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.downstream.scheme.label"),field:"downstreamScheme"},{default:l(()=>[e(h,{modelValue:o.value.downstreamScheme,"onUpdate:modelValue":t[4]||(t[4]=d=>o.value.downstreamScheme=d),"allow-clear":"",placeholder:a.$t("gc.downstream.scheme.placeholder")},{default:l(()=>[e(n,{value:"http"},{default:l(()=>[me]),_:1}),e(n,{value:"https"},{default:l(()=>[ve]),_:1})]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.downstream.httpVersion.label"),field:"downstreamHttpVersion"},{default:l(()=>[e(h,{modelValue:o.value.downstreamHttpVersion,"onUpdate:modelValue":t[5]||(t[5]=d=>o.value.downstreamHttpVersion=d),"allow-clear":"",placeholder:a.$t("gc.downstream.httpVersion.placeholder")},{default:l(()=>[e(n,{value:"1.0"},{default:l(()=>[ce]),_:1}),e(n,{value:"1.1"},{default:l(()=>[fe]),_:1}),e(n,{value:"2.0"},{default:l(()=>[be]),_:1})]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})]),_:1}),e(y,{class:"general-card",bordered:!1},{title:l(()=>[r(u(a.$t("gc.loadbalancer.title")),1)]),default:l(()=>[o.value.loadBalancerOptions?(c(),b(m,{key:0,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.loadbalancer.type.label"),field:"loadBalancerOptions.type"},{default:l(()=>[e(h,{modelValue:o.value.loadBalancerOptions.type,"onUpdate:modelValue":t[6]||(t[6]=d=>o.value.loadBalancerOptions.type=d),"allow-clear":"",placeholder:a.$t("gc.loadbalancer.type.placeholder")},{default:l(()=>[e(n,{value:"LeastConnection"},{default:l(()=>[r(u(a.$t("gc.loadbalancer.type.least.connection.label")),1)]),_:1}),e(n,{value:"RoundRobin"},{default:l(()=>[r(u(a.$t("gc.loadbalancer.type.round.robin.label")),1)]),_:1}),e(n,{value:"NoLoadBalance"},{default:l(()=>[r(u(a.$t("gc.loadbalancer.type.no.load.balance.label")),1)]),_:1}),e(n,{value:"CookieStickySessions"},{default:l(()=>[r(u(a.$t("gc.loadbalancer.type.cookie.sticky.sessions.label")),1)]),_:1})]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.loadbalancer.key.label"),field:"loadBalancerOptions.key"},{default:l(()=>[e(f,{modelValue:o.value.loadBalancerOptions.key,"onUpdate:modelValue":t[7]||(t[7]=d=>o.value.loadBalancerOptions.key=d),placeholder:a.$t("gc.loadbalancer.key.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.loadbalancer.expiry.label"),field:"loadBalancerOptions.expiry"},{default:l(()=>[e(v,{modelValue:o.value.loadBalancerOptions.expiry,"onUpdate:modelValue":t[8]||(t[8]=d=>o.value.loadBalancerOptions.expiry=d),"hide-button":!0,placeholder:a.$t("gc.loadbalancer.expiry.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0)]),_:1}),e(y,{class:"general-card"},{title:l(()=>[r(u(a.$t("gc.service.discovery.title")),1)]),default:l(()=>[o.value.serviceDiscoveryProviderOptions?(c(),b(m,{key:0,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("radio.label"),field:"serviceDiscoveryProviderOptions.enabled"},{default:l(()=>[e(V,{modelValue:o.value.serviceDiscoveryProviderOptions.enabled,"onUpdate:modelValue":t[9]||(t[9]=d=>o.value.serviceDiscoveryProviderOptions.enabled=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.enabled.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.disabled.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.scheme.label"),field:"serviceDiscoveryProviderOptions.scheme"},{default:l(()=>[e(h,{modelValue:o.value.serviceDiscoveryProviderOptions.scheme,"onUpdate:modelValue":t[10]||(t[10]=d=>o.value.serviceDiscoveryProviderOptions.scheme=d),"allow-clear":"",placeholder:a.$t("gc.service.discovery.scheme.placeholder")},{default:l(()=>[e(n,{value:"http"},{default:l(()=>[ge]),_:1}),e(n,{value:"https"},{default:l(()=>[ye]),_:1})]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.host.label"),field:"serviceDiscoveryProviderOptions.host"},{default:l(()=>[e(f,{modelValue:o.value.serviceDiscoveryProviderOptions.host,"onUpdate:modelValue":t[11]||(t[11]=d=>o.value.serviceDiscoveryProviderOptions.host=d),placeholder:a.$t("gc.service.discovery.host.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.port.label"),field:"serviceDiscoveryProviderOptions.port"},{default:l(()=>[e(v,{modelValue:o.value.serviceDiscoveryProviderOptions.port,"onUpdate:modelValue":t[12]||(t[12]=d=>o.value.serviceDiscoveryProviderOptions.port=d),"hide-button":!0,placeholder:a.$t("gc.service.discovery.port.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0),o.value.serviceDiscoveryProviderOptions?(c(),b(m,{key:1,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.type.label"),field:"serviceDiscoveryProviderOptions.type"},{default:l(()=>[e(h,{modelValue:o.value.serviceDiscoveryProviderOptions.type,"onUpdate:modelValue":t[13]||(t[13]=d=>o.value.serviceDiscoveryProviderOptions.type=d),"allow-clear":"",placeholder:a.$t("gc.service.discovery.type.placeholder")},{default:l(()=>[e(n,{value:"Consul"},{default:l(()=>[Ve]),_:1}),e(n,{value:"PollConsul"},{default:l(()=>[he]),_:1}),e(n,{value:"Eureka"},{default:l(()=>[Oe]),_:1})]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.token.label"),field:"serviceDiscoveryProviderOptions.token"},{default:l(()=>[e(f,{modelValue:o.value.serviceDiscoveryProviderOptions.token,"onUpdate:modelValue":t[14]||(t[14]=d=>o.value.serviceDiscoveryProviderOptions.token=d),placeholder:a.$t("gc.service.discovery.token.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.configuration.key.label"),field:"serviceDiscoveryProviderOptions.configurationKey"},{default:l(()=>[e(f,{modelValue:o.value.serviceDiscoveryProviderOptions.configurationKey,"onUpdate:modelValue":t[15]||(t[15]=d=>o.value.serviceDiscoveryProviderOptions.configurationKey=d),placeholder:a.$t("gc.service.discovery.configuration.key.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.namespace.label"),field:"serviceDiscoveryProviderOptions.namespace"},{default:l(()=>[e(f,{modelValue:o.value.serviceDiscoveryProviderOptions.namespace,"onUpdate:modelValue":t[16]||(t[16]=d=>o.value.serviceDiscoveryProviderOptions.namespace=d),placeholder:a.$t("gc.service.discovery.namespace.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0),o.value.serviceDiscoveryProviderOptions?(c(),b(m,{key:2,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.service.discovery.polling.interval.label"),field:"serviceDiscoveryProviderOptions.pollingInterval"},{default:l(()=>[e(v,{modelValue:o.value.serviceDiscoveryProviderOptions.pollingInterval,"onUpdate:modelValue":t[17]||(t[17]=d=>o.value.serviceDiscoveryProviderOptions.pollingInterval=d),"hide-button":!0,placeholder:a.$t("gc.service.discovery.polling.interval.placeholder")},{append:l(()=>[r(u(a.$t("unit.ms")),1)]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0)]),_:1}),e(y,{class:"general-card",bordered:!1},{title:l(()=>[r(u(a.$t("gc.qos.title")),1)]),default:l(()=>[o.value.qoSOptions?(c(),b(m,{key:0,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("radio.label"),field:"qoSOptions.enabled"},{default:l(()=>[e(V,{modelValue:o.value.qoSOptions.enabled,"onUpdate:modelValue":t[18]||(t[18]=d=>o.value.qoSOptions.enabled=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.enabled.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.disabled.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.qos.exceptions.allowed.before.breaking.label"),field:"QosExceptionsallowedbeforebreaking"},{default:l(()=>[e(v,{modelValue:o.value.qoSOptions.exceptionsAllowedBeforeBreaking,"onUpdate:modelValue":t[19]||(t[19]=d=>o.value.qoSOptions.exceptionsAllowedBeforeBreaking=d),"hide-button":!0,placeholder:a.$t("gc.qos.exceptions.allowed.before.breaking.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.qos.duration.of.break.label"),field:"QosDurationofbreak"},{default:l(()=>[e(v,{modelValue:o.value.qoSOptions.durationOfBreak,"onUpdate:modelValue":t[20]||(t[20]=d=>o.value.qoSOptions.durationOfBreak=d),"hide-button":!0,placeholder:a.$t("gc.qos.duration.of.break.placeholder")},{append:l(()=>[$e]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.qos.timeout.value.label"),field:"QosTimeoutvalue"},{default:l(()=>[e(v,{modelValue:o.value.qoSOptions.timeoutValue,"onUpdate:modelValue":t[21]||(t[21]=d=>o.value.qoSOptions.timeoutValue=d),"hide-button":!0,placeholder:a.$t("gc.qos.timeout.value.placeholder")},{append:l(()=>[_e]),_:1},8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0)]),_:1}),e(y,{class:"general-card",bordered:!1},{title:l(()=>[r(u(a.$t("gc.rateLimit.title")),1)]),default:l(()=>[o.value.rateLimitOptions?(c(),b(m,{key:0,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("radio.label"),field:"rateLimitOptions.enableRateLimiting"},{default:l(()=>[e(V,{modelValue:o.value.rateLimitOptions.enableRateLimiting,"onUpdate:modelValue":t[22]||(t[22]=d=>o.value.rateLimitOptions.enableRateLimiting=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.enabled.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.disabled.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.rateLimit.period.label"),field:"rateLimitOptions.period"},{default:l(()=>[e(v,{modelValue:o.value.rateLimitOptions.period,"onUpdate:modelValue":t[23]||(t[23]=d=>o.value.rateLimitOptions.period=d),placeholder:a.$t("gc.rateLimit.period.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.rateLimit.periodTimespan.label"),field:"rateLimitOptions.periodTimespan"},{default:l(()=>[e(v,{modelValue:o.value.rateLimitOptions.periodTimespan,"onUpdate:modelValue":t[24]||(t[24]=d=>o.value.rateLimitOptions.periodTimespan=d),placeholder:a.$t("gc.rateLimit.periodTimespan.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0),o.value.rateLimitOptions?(c(),b(m,{key:1,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.rateLimit.limit.label"),field:"rateLimitOptions.limit"},{default:l(()=>[e(v,{modelValue:o.value.rateLimitOptions.limit,"onUpdate:modelValue":t[25]||(t[25]=d=>o.value.rateLimitOptions.limit=d),placeholder:a.$t("gc.rateLimit.limit.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.rateLimit.clientWhitelist.label"),field:"rateLimitOptions.clientWhitelist"},{default:l(()=>[e(f,{modelValue:o.value.rateLimitOptions.clientWhitelistStr,"onUpdate:modelValue":t[26]||(t[26]=d=>o.value.rateLimitOptions.clientWhitelistStr=d),placeholder:a.$t("gc.rateLimit.clientWhitelist.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0)]),_:1}),e(y,{class:"general-card",bordered:!1},{title:l(()=>[r(u(a.$t("gc.httpHandler.title")),1)]),default:l(()=>[o.value.httpHandlerOptions?(c(),b(m,{key:0,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.httpHandler.allow.auto.redirect.label"),field:"httpHandlerOptions.allowAutoRedirect"},{default:l(()=>[e(V,{modelValue:o.value.httpHandlerOptions.allowAutoRedirect,"onUpdate:modelValue":t[27]||(t[27]=d=>o.value.httpHandlerOptions.allowAutoRedirect=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.allow.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.not.allow.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.httpHandler.use.cookie.container.label"),field:"httpHandlerOptions.useCookieContainer"},{default:l(()=>[e(V,{modelValue:o.value.httpHandlerOptions.useCookieContainer,"onUpdate:modelValue":t[28]||(t[28]=d=>o.value.httpHandlerOptions.useCookieContainer=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.use.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.not.use.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.httpHandler.use.tracing.label"),field:"httpHandlerOptions.useTracing"},{default:l(()=>[e(V,{modelValue:o.value.httpHandlerOptions.useTracing,"onUpdate:modelValue":t[29]||(t[29]=d=>o.value.httpHandlerOptions.useTracing=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.use.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.not.use.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1}),e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.httpHandler.use.proxy.label"),field:"httpHandlerOptions.useProxy"},{default:l(()=>[e(V,{modelValue:o.value.httpHandlerOptions.useProxy,"onUpdate:modelValue":t[30]||(t[30]=d=>o.value.httpHandlerOptions.useProxy=d)},{default:l(()=>[e(p,{value:!0},{default:l(()=>[r(u(a.$t("radio.use.label")),1)]),_:1}),e(p,{value:!1},{default:l(()=>[r(u(a.$t("radio.not.use.label")),1)]),_:1})]),_:1},8,["modelValue"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0),o.value.httpHandlerOptions?(c(),b(m,{key:1,gutter:{md:8,lg:24,xl:32}},{default:l(()=>[e(s,{sm:24,md:12,lg:8,xl:6},{default:l(()=>[e(i,{label:a.$t("gc.httpHandler.max.connections.per.server.label"),field:"httpHandlerOptions.maxConnectionsPerServer"},{default:l(()=>[e(v,{modelValue:o.value.httpHandlerOptions.maxConnectionsPerServer,"onUpdate:modelValue":t[31]||(t[31]=d=>o.value.httpHandlerOptions.maxConnectionsPerServer=d),"hide-button":!0,placeholder:a.$t("gc.httpHandler.max.connections.per.server.placeholder")},null,8,["modelValue","placeholder"])]),_:1},8,["label"])]),_:1})]),_:1})):g("",!0)]),_:1})]),_:1}),W("div",ke,[e(U,null,{default:l(()=>[e(S,{type:"primary",loading:F(C),onClick:B},{default:l(()=>[r(u(a.$t("submit")),1)]),_:1},8,["loading"])]),_:1})])]),_:1},8,["model"])])}}});var Ne=R(De,[["__scopeId","data-v-204d45aa"]]);export{Ne as default};