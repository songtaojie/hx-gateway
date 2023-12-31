<template>
  <div class="container">
    <Breadcrumb :items="['menu.globalconfiguration', 'menu.globalconfiguration.create']" />
    <a-form ref="formRef" :model="formData">
      <a-space direction="vertical" :size="16">
        <a-card class="general-card">
          <template #title>
            {{ $t('gc.basic.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item field="projectId" :label="$t('project.label.name')" :rules="[{ required: true, message: $t('project.form.name.required.errmsg') }]">
                <a-select v-model="formData.projectId" allow-clear :options="projectOptions" :field-names="{ value: 'id', label: 'name' }" :placeholder="$t('select.options.default')" />
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.basic.name.label')" field="name" :rules="[{ required: true, message: $t('gc.form.name.required.errmsg') }]">
                <a-input v-model="formData.name" :placeholder="$t('gc.basic.name.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.basic.baseUrl.label')" field="baseUrl">
                <a-input v-model="formData.baseUrl" :placeholder="$t('gc.basic.baseUrl.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.basic.requestIdKey.label')" field="requestIdKey" :rules="[{ required: true, message: $t('gc.form.requestIdKey.required.errmsg') }]">
                <a-input v-model="formData.requestIdKey" :placeholder="$t('gc.basic.requestIdKey.placeholder')"></a-input>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card">
          <template #title>
            {{ $t('gc.downstream.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.downstream.scheme.label')" field="downstreamScheme">
                <a-select v-model="formData.downstreamScheme" allow-clear :placeholder="$t('gc.downstream.scheme.placeholder')">
                  <a-option value="http">http</a-option>
                  <a-option value="https">https</a-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.downstream.httpVersion.label')" field="downstreamHttpVersion">
                <a-select v-model="formData.downstreamHttpVersion" allow-clear :placeholder="$t('gc.downstream.httpVersion.placeholder')">
                  <a-option value="1.0">1.0</a-option>
                  <a-option value="1.1">1.1</a-option>
                  <a-option value="2.0">2.0</a-option>
                </a-select>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card" :bordered="false">
          <template #title>
            {{ $t('gc.loadbalancer.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.loadBalancerOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.loadbalancer.type.label')" field="loadBalancerOptions.type">
                <a-select v-model="formData.loadBalancerOptions.type" allow-clear :placeholder="$t('gc.loadbalancer.type.placeholder')">
                  <a-option value="LeastConnection">{{ $t('gc.loadbalancer.type.least.connection.label') }}</a-option>
                  <a-option value="RoundRobin">{{ $t('gc.loadbalancer.type.round.robin.label') }}</a-option>
                  <a-option value="NoLoadBalance">{{ $t('gc.loadbalancer.type.no.load.balance.label') }}</a-option>
                  <a-option value="CookieStickySessions">{{ $t('gc.loadbalancer.type.cookie.sticky.sessions.label') }}</a-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.loadbalancer.key.label')" field="loadBalancerOptions.key">
                <a-input v-model="formData.loadBalancerOptions.key" :placeholder="$t('gc.loadbalancer.key.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.loadbalancer.expiry.label')" field="loadBalancerOptions.expiry">
                <a-input-number v-model="formData.loadBalancerOptions.expiry" :hide-button="true" :placeholder="$t('gc.loadbalancer.expiry.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card">
          <template #title>
            {{ $t('gc.service.discovery.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.serviceDiscoveryProviderOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('radio.label')" field="serviceDiscoveryProviderOptions.enabled">
                <a-radio-group v-model="formData.serviceDiscoveryProviderOptions.enabled">
                  <a-radio :value="true">{{ $t('radio.enabled.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.disabled.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.scheme.label')" field="serviceDiscoveryProviderOptions.scheme">
                <a-select v-model="formData.serviceDiscoveryProviderOptions.scheme" allow-clear :placeholder="$t('gc.service.discovery.scheme.placeholder')">
                  <a-option value="http">http</a-option>
                  <a-option value="https">https</a-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.host.label')" field="serviceDiscoveryProviderOptions.host">
                <a-input v-model="formData.serviceDiscoveryProviderOptions.host" :placeholder="$t('gc.service.discovery.host.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.port.label')" field="serviceDiscoveryProviderOptions.port">
                <a-input-number v-model="formData.serviceDiscoveryProviderOptions.port" :hide-button="true" :placeholder="$t('gc.service.discovery.port.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.serviceDiscoveryProviderOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.type.label')" field="serviceDiscoveryProviderOptions.type">
                <a-select v-model="formData.serviceDiscoveryProviderOptions.type" allow-clear :placeholder="$t('gc.service.discovery.type.placeholder')">
                  <a-option value="Consul">Consul</a-option>
                  <a-option value="PollConsul">PollConsul</a-option>
                  <a-option value="Eureka">Eureka</a-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.token.label')" field="serviceDiscoveryProviderOptions.token">
                <a-input v-model="formData.serviceDiscoveryProviderOptions.token" :placeholder="$t('gc.service.discovery.token.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.configuration.key.label')" field="serviceDiscoveryProviderOptions.configurationKey">
                <a-input v-model="formData.serviceDiscoveryProviderOptions.configurationKey" :placeholder="$t('gc.service.discovery.configuration.key.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.namespace.label')" field="serviceDiscoveryProviderOptions.namespace">
                <a-input v-model="formData.serviceDiscoveryProviderOptions.namespace" :placeholder="$t('gc.service.discovery.namespace.placeholder')"></a-input>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.serviceDiscoveryProviderOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.service.discovery.polling.interval.label')" field="serviceDiscoveryProviderOptions.pollingInterval">
                <a-input-number v-model="formData.serviceDiscoveryProviderOptions.pollingInterval" :hide-button="true" :placeholder="$t('gc.service.discovery.polling.interval.placeholder')">
                  <template #append>{{ $t('unit.ms') }}</template>
                </a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card" :bordered="false">
          <template #title>
            {{ $t('gc.qos.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.qoSOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('radio.label')" field="qoSOptions.enabled">
                <a-radio-group v-model="formData.qoSOptions.enabled">
                  <a-radio :value="true">{{ $t('radio.enabled.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.disabled.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.qos.exceptions.allowed.before.breaking.label')" field="QosExceptionsallowedbeforebreaking">
                <a-input-number v-model="formData.qoSOptions.exceptionsAllowedBeforeBreaking" :hide-button="true" :placeholder="$t('gc.qos.exceptions.allowed.before.breaking.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.qos.duration.of.break.label')" field="QosDurationofbreak">
                <a-input-number v-model="formData.qoSOptions.durationOfBreak" :hide-button="true" :placeholder="$t('gc.qos.duration.of.break.placeholder')">
                  <template #append>ms</template>
                </a-input-number>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.qos.timeout.value.label')" field="QosTimeoutvalue">
                <a-input-number v-model="formData.qoSOptions.timeoutValue" :hide-button="true" :placeholder="$t('gc.qos.timeout.value.placeholder')">
                  <template #append>ms</template>
                </a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card" :bordered="false">
          <template #title>
            {{ $t('gc.rateLimit.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.rateLimitOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('radio.label')" field="rateLimitOptions.enableRateLimiting">
                <a-radio-group v-model="formData.rateLimitOptions.enableRateLimiting">
                  <a-radio :value="true">{{ $t('radio.enabled.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.disabled.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.rateLimit.period.label')" field="rateLimitOptions.period">
                <a-input-number v-model="formData.rateLimitOptions.period" :placeholder="$t('gc.rateLimit.period.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.rateLimit.periodTimespan.label')" field="rateLimitOptions.periodTimespan">
                <a-input-number v-model="formData.rateLimitOptions.periodTimespan" :placeholder="$t('gc.rateLimit.periodTimespan.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.rateLimitOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.rateLimit.limit.label')" field="rateLimitOptions.limit">
                <a-input-number v-model="formData.rateLimitOptions.limit" :placeholder="$t('gc.rateLimit.limit.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.rateLimit.clientWhitelist.label')" field="rateLimitOptions.clientWhitelist">
                <a-input v-model="formData.rateLimitOptions.clientWhitelistStr" :placeholder="$t('gc.rateLimit.clientWhitelist.placeholder')"></a-input>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card" :bordered="false">
          <template #title>
            {{ $t('gc.httpHandler.title') }}
          </template>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.httpHandlerOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.httpHandler.allow.auto.redirect.label')" field="httpHandlerOptions.allowAutoRedirect">
                <a-radio-group v-model="formData.httpHandlerOptions.allowAutoRedirect">
                  <a-radio :value="true">{{ $t('radio.allow.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.allow.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.httpHandler.use.cookie.container.label')" field="httpHandlerOptions.useCookieContainer">
                <a-radio-group v-model="formData.httpHandlerOptions.useCookieContainer">
                  <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.httpHandler.use.tracing.label')" field="httpHandlerOptions.useTracing">
                <a-radio-group v-model="formData.httpHandlerOptions.useTracing">
                  <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.httpHandler.use.proxy.label')" field="httpHandlerOptions.useProxy">
                <a-radio-group v-model="formData.httpHandlerOptions.useProxy">
                  <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="{ md: 8, lg: 24, xl: 32 }" v-if="formData.httpHandlerOptions">
            <a-col :sm="24" :md="12" :lg="8" :xl="6">
              <a-form-item :label="$t('gc.httpHandler.max.connections.per.server.label')" field="httpHandlerOptions.maxConnectionsPerServer">
                <a-input-number v-model="formData.httpHandlerOptions.maxConnectionsPerServer" :hide-button="true" :placeholder="$t('gc.httpHandler.max.connections.per.server.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
      </a-space>
      <div class="actions">
        <a-space>
          <a-button type="primary" :loading="loading" @click="onSubmitClick">
            {{ $t('submit') }}
          </a-button>
        </a-space>
      </div>
    </a-form>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { isEmpty } from 'lodash'
import { useI18n } from 'vue-i18n'
import { FormInstance } from '@arco-design/web-vue/es/form'
import useLoading from '@/hooks/loading'
import { getList as getProjectList, ProjectResponse } from '@/api/project'
import { GlobalConfigurationModel, createGlobalConfigurationModel, add, update, getDetail } from '@/api/global-configuration'
import { Message } from '@arco-design/web-vue'
import { useRouter, useRoute } from 'vue-router'
const router = useRouter()
const route = useRoute()
const { t } = useI18n()
const formData = ref<GlobalConfigurationModel>(createGlobalConfigurationModel())
const formRef = ref<FormInstance>()
const projectOptions = ref<ProjectResponse[]>([])
const { loading, setLoading } = useLoading()
const onSubmitClick = async () => {
  formRef.value &&
    formRef.value.validate(async (r: any) => {
      if (r == void 0) {
        setLoading(true)
        var result = false
        if (isEmpty(formData.value.id)) {
          const { data } = await add(formData.value)
          result = data
        } else {
          const { data } = await update(formData.value)
          result = data
        }
        if (result) {
          Message.success({
            content: t('submit.success'),
            duration: 5 * 1000
          })
          router.push({ name: 'global-search' })
        } else {
          Message.error({
            content: t('submit.fail'),
            duration: 5 * 1000
          })
        }
        setLoading(false)
      }
    })
}
const fetchData = async () => {
  try {
    const id = route.query.id as string
    if (!isEmpty(id)) {
      const { data } = await getDetail(id)
      formData.value = data
    }
  } catch (err) {
    console.log(err)
  }
}
fetchData()
onMounted(async () => {
  const { data } = await getProjectList()
  if (data) {
    projectOptions.value = data
    console.log(projectOptions.value, 'projectOptions')
  }
})
</script>

<style scoped lang="less">
.container {
  padding: 0 20px 40px 20px;
  overflow: hidden;
}

.actions {
  position: fixed;
  left: 0;
  right: 0;
  bottom: 0;
  height: 60px;
  padding: 14px 20px 14px 0;
  background: var(--color-bg-2);
  text-align: right;
}
</style>
