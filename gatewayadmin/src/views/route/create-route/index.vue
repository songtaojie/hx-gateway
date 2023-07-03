<template>
  <div class="container">
    <Breadcrumb :items="['menu.project', 'menu.route.edit']" />
    <a-form ref="formRef" layout="vertical" :model="formData">
      <a-space direction="vertical" :size="16">
        <a-card class="general-card">
          <template #title>
            {{ $t('route.basic.title') }}
          </template>
          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('radio.label')" field="status">
                <a-radio-group v-model="formData.status">
                  <a-radio :value="StatusEnum.Enable">{{ $t('radio.enabled.label') }}</a-radio>
                  <a-radio :value="StatusEnum.Disable">{{ $t('radio.disabled.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.requestIdKey.label')" :rules="[{ required: true, message: 'requestIdKey is required' }]" field="requestIdKey">
                <a-input v-model="formData.requestIdKey" :placeholder="$t('route.requestIdKey.placeholder')"></a-input>
              </a-form-item>
            </a-col>
          </a-row>

          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.upstream.host.label')" :rules="[{ required: true, message: 'upstreamHost is required' }]" field="upstreamHost">
                <a-input v-model="formData.upstreamHost" :placeholder="$t('route.upstream.host.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.upstream.path.template.label')" field="upstreamPathTemplate">
                <a-input v-model="formData.upstreamPathTemplate" :placeholder="$t('route.upstream.path.template.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.upstream.http.method.label')" field="upstreamHttpMethod">
                <a-checkbox-group v-model="formData.upstreamHttpMethod">
                  <a-checkbox value="Get">GET</a-checkbox>
                  <a-checkbox value="Post">POST</a-checkbox>
                  <a-checkbox value="Put">PUT</a-checkbox>
                  <a-checkbox value="Delete">DELETE</a-checkbox>
                  <a-checkbox value="Patch">PATCH</a-checkbox>
                  <a-checkbox value="Option">OPTION</a-checkbox>
                </a-checkbox-group>
              </a-form-item>
            </a-col>
          </a-row>

          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.downstream.http.version')" field="downstreamHttpVersion">
                <a-select v-model="formData.downstreamHttpVersion" allow-clear :placeholder="$t('route.downstream.http.version.placeholder')">
                  <a-option value="1.0">1.0</a-option>
                  <a-option value="1.1">1.1</a-option>
                  <a-option value="2.0">2.0</a-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.downstream.scheme.label')" field="downstreamScheme">
                <a-select v-model="formData.downstreamScheme" allow-clear :placeholder="$t('route.downstream.scheme.placeholder')">
                  <a-option value="http">http</a-option>
                  <a-option value="https">https</a-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.downstream.http.method.label')" field="downstreamHttpMethod">
                <a-select v-model="formData.downstreamHttpMethod" allow-clear :placeholder="$t('route.downstream.http.method.placeholder')">
                  <a-option value="Get">GET</a-option>
                  <a-option value="Post">POST</a-option>
                  <a-option value="Put">PUT</a-option>
                  <a-option value="Delete">DELETE</a-option>
                  <a-option value="Patch">PATCH</a-option>
                  <a-option value="Option">OPTION</a-option>
                </a-select>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.downstream.path.template.label')" field="downstreamPathTemplate">
                <a-input v-model="formData.downstreamPathTemplate" :placeholder="$t('route.downstream.path.template.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :span="8" v-if="formData.fileCacheOptions">
              <a-form-item :label="$t('route.filecache.ttlseconds.label')" field="cacheTtlseconds">
                <a-input-number v-model="formData.fileCacheOptions.ttlSeconds" :hide-button="true" :placeholder="$t('route.filecache.ttlseconds.placeholder')">
                  <template #append>{{ $t('unit.second') }}</template>
                </a-input-number>
              </a-form-item>
            </a-col>
            <a-col :span="8" v-if="formData.fileCacheOptions">
              <a-form-item :label="$t('route.filecache.region.label')" field="cacheRegion">
                <a-input v-model="formData.fileCacheOptions.region" :placeholder="$t('route.filecache.region.placeholder')"></a-input>
              </a-form-item>
            </a-col>
          </a-row>

          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.delegating.handlers.label')" field="delegatingHandlers">
                <!-- <a-input v-model="formData.delegatingHandlers" :placeholder="$t('route.delegating.handlers')"></a-input> -->
                <a-space wrap>
                  <a-tag v-for="tag of formData.delegatingHandlers" :key="tag" size="large" :closable="true" @close="handleRemoveDelegatingHandlersTag(tag)">
                    {{ tag }}
                  </a-tag>

                  <a-input v-if="showDelegatingHandlersInput" ref="inputDelegatingHandlersRef" v-model.trim="inputDelegatingHandlersVal" :style="{ width: '90px' }" @keyup.enter="handleAddDelegatingHandlersTag" @blur="handleAddDelegatingHandlersTag" />
                  <a-tag
                    v-else
                    size="large"
                    :style="{
                      width: '120px',
                      backgroundColor: 'var(--color-fill-2)',
                      border: '1px dashed var(--color-fill-3)',
                      cursor: 'pointer'
                    }"
                    @click="handleEditDelegatingHandlersTag"
                  >
                    <template #icon>
                      <icon-plus />
                    </template>
                    {{ $t('rate.delegating.handlers.placeholder') }}
                  </a-tag>
                </a-space>
              </a-form-item>
            </a-col>
          </a-row>

          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.routeIsCaseSensitive.label')" field="routeIsCaseSensitive">
                <a-radio-group v-model="formData.routeIsCaseSensitive">
                  <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.dangerousAcceptAnyServerCertificateValidator.label')" field="dangerousAcceptAnyServerCertificateValidator">
                <a-radio-group v-model="formData.dangerousAcceptAnyServerCertificateValidator">
                  <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.useServiceDiscovery.label')" field="useServiceDiscovery">
                <a-radio-group v-model="formData.useServiceDiscovery">
                  <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                  <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                </a-radio-group>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.service.namespace.label')" field="serviceNamespace">
                <a-input v-model="formData.serviceNamespace" :disabled="!formData.useServiceDiscovery" :placeholder="$t('route.service.namespace.placeholder')"></a-input>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.service.name.label')" field="serviceName">
                <a-input v-model="formData.serviceName" :disabled="!formData.useServiceDiscovery" :placeholder="$t('route.service.name.placeholder')"></a-input>
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="80">
            <a-col :span="8">
              <a-form-item :label="$t('route.priority.label')" field="priority">
                <a-input-number v-model="formData.priority" :hide-button="true" :placeholder="$t('route.priority.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
            <a-col :span="8">
              <a-form-item :label="$t('route.sort.label')" field="sort">
                <a-input-number v-model="formData.sort" :placeholder="$t('route.sort.placeholder')"></a-input-number>
              </a-form-item>
            </a-col>
          </a-row>
        </a-card>
        <a-card class="general-card" :bordered="false">
          <a-tabs default-active-key="1">
            <a-tab-pane key="1" :title="$t('route.downstreamHostAndPorts.title')">
              <a-row style="margin-bottom: 16px">
                <a-col :span="16">
                  <a-space>
                    <a-button type="primary" @click="handleEdit(true, undefined)">
                      <template #icon>
                        <icon-plus />
                      </template>
                      {{ $t('add.operations.table') }}
                    </a-button>
                  </a-space>
                </a-col>
              </a-row>
              <a-table row-key="id" :loading="loading" :data="formData.downstreamHostAndPorts" :bordered="false">
                <template #columns>
                  <a-table-column :title="$t('route.downstreamHostAndPorts.host.table')" data-index="host" />
                  <a-table-column :title="$t('route.downstreamHostAndPorts.port.table')" data-index="port" />
                  <a-table-column :title="$t('operations.table')" data-index="operations">
                    <template #cell="{ record }">
                      <a-space>
                        <a-button type="outline" size="small" @click="handleEdit(false, record)">
                          {{ $t('edit.operations.table') }}
                        </a-button>
                        <a-button type="primary" size="small" status="danger" @click="handleDelete(record)">
                          {{ $t('del.operations.table') }}
                        </a-button>
                      </a-space>
                    </template>
                  </a-table-column>
                </template>
              </a-table>
            </a-tab-pane>
            <a-tab-pane key="2" :title="$t('route.qos.title')">
              <a-row :gutter="80" v-if="formData.qoSOptions">
                <a-col :span="8">
                  <a-form-item :label="$t('route.qos.exceptions.allowed.before.breaking.label')">
                    <a-input-number v-model="formData.qoSOptions.exceptionsAllowedBeforeBreaking" :placeholder="$t('route.qos.exceptions.allowed.before.breaking.placeholder')" autocomplete="off"></a-input-number>
                  </a-form-item>
                </a-col>
                <a-col :span="8">
                  <a-form-item :label="$t('route.qos.duration.of.break.label')">
                    <a-input-number v-model="formData.qoSOptions.durationOfBreak" :placeholder="$t('route.qos.duration.of.break.placeholder')" autocomplete="off">
                      <template #append>{{ $t('unit.ms') }}</template>
                    </a-input-number>
                  </a-form-item>
                </a-col>
                <a-col :span="8">
                  <a-form-item :label="$t('route.qos.timeout.value.label')">
                    <a-input-number v-model="formData.qoSOptions.timeoutValue" :placeholder="$t('route.qos.timeout.value.placeholder')" autocomplete="off">
                      <template #append>{{ $t('unit.ms') }}</template>
                    </a-input-number>
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane key="3" :title="$t('route.rate.limit.title')">
              <a-row :gutter="80" v-if="formData.rateLimitOptions">
                <a-col :span="6">
                  <a-form-item :label="$t('route.rate.limit.enableRateLimiting.label')" field="enableRateLimiting">
                    <a-radio-group v-model="formData.rateLimitOptions.enableRateLimiting">
                      <a-radio :value="true">{{ $t('radio.enabled.label') }}</a-radio>
                      <a-radio :value="false">{{ $t('radio.disabled.label') }}</a-radio>
                    </a-radio-group>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.rate.limit.period.label')">
                    <a-input-number v-model="formData.rateLimitOptions.period" :placeholder="$t('route.rate.limit.period.placeholder')" autocomplete="off"></a-input-number>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.rate.limit.periodTimespan.label')">
                    <a-input-number v-model="formData.rateLimitOptions.periodTimespan" :placeholder="$t('route.rate.limit.periodTimespan.placeholder')" autocomplete="off"></a-input-number>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.rate.limit.limit.label')">
                    <a-input-number v-model="formData.rateLimitOptions.limit" :placeholder="$t('route.rate.limit.limit.label.placeholder')" autocomplete="off"></a-input-number>
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="80">
                <a-col :span="24">
                  <a-form-item :label="$t('route.rate.limit.clientWhitelist.label')">
                    <a-space wrap>
                      <a-tag v-for="tag of formData.rateLimitOptions?.clientWhitelist" :key="tag" size="large" :closable="true" @close="handleRemoveWhiteListTag(tag)">
                        {{ tag }}
                      </a-tag>

                      <a-input v-if="showWhiteListInput" ref="inputWhiteListRef" v-model.trim="inputWhiteListVal" :style="{ width: '90px' }" @keyup.enter="handleAddWhiteListTag" @blur="handleAddWhiteListTag" />
                      <a-tag
                        v-else
                        size="large"
                        :style="{
                          width: '120px',
                          backgroundColor: 'var(--color-fill-2)',
                          border: '1px dashed var(--color-fill-3)',
                          cursor: 'pointer'
                        }"
                        @click="handleEditWhiteListTag"
                      >
                        <template #icon>
                          <icon-plus />
                        </template>
                        {{ $t('route.rate.limit.clientWhitelist.placeholder') }}
                      </a-tag>
                    </a-space>
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane key="4" :title="$t('route.authentication.title')">
              <a-row :gutter="80" v-if="formData.authenticationOptions">
                <a-col :span="8">
                  <a-form-item :label="$t('route.authentication.provider.key.label')">
                    <a-input v-model="formData.authenticationOptions.authenticationProviderKey" :placeholder="$t('route.authentication.provider.key.placeholder')" autocomplete="off"></a-input>
                  </a-form-item>
                </a-col>
                <a-col :span="16">
                  <a-form-item :label="$t('route.authentication.allowed.scopes.label')">
                    <a-space wrap>
                      <a-tag v-for="tag of formData.authenticationOptions.AllowedScopes" :key="tag" size="large" :closable="true" @close="handleRemoveScopeTag(tag)">
                        {{ tag }}
                      </a-tag>

                      <a-input v-if="showScopeInput" ref="inputScopeRef" v-model.trim="inputScopeVal" :style="{ width: '90px' }" @keyup.enter="handleAddScopeTag" @blur="handleAddScopeTag" />
                      <a-tag
                        v-else
                        size="large"
                        :style="{
                          width: '90px',
                          backgroundColor: 'var(--color-fill-2)',
                          border: '1px dashed var(--color-fill-3)',
                          cursor: 'pointer'
                        }"
                        @click="handleEditScopeTag"
                      >
                        <template #icon>
                          <icon-plus />
                        </template>
                        {{ $t('route.authentication.allowed.scopes.placeholder') }}
                      </a-tag>
                    </a-space>
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane key="5" :title="$t('route.http.handler.title')">
              <a-row :gutter="80" v-if="formData.httpHandlerOptions">
                <a-col :span="6">
                  <a-form-item :label="$t('route.http.handler.allow.auto.redirect.label')">
                    <a-radio-group v-model="formData.httpHandlerOptions.allowAutoRedirect">
                      <a-radio :value="true">{{ $t('radio.allow.label') }}</a-radio>
                      <a-radio :value="false">{{ $t('radio.not.allow.label') }}</a-radio>
                    </a-radio-group>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.http.handler.use.cookie.container.label')">
                    <a-radio-group v-model="formData.httpHandlerOptions.useCookieContainer">
                      <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                      <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                    </a-radio-group>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.http.handler.use.tracing.label')">
                    <a-radio-group v-model="formData.httpHandlerOptions.useTracing">
                      <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                      <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                    </a-radio-group>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.http.handler.use.proxy.label')">
                    <a-radio-group v-model="formData.httpHandlerOptions.useProxy">
                      <a-radio :value="true">{{ $t('radio.use.label') }}</a-radio>
                      <a-radio :value="false">{{ $t('radio.not.use.label') }}</a-radio>
                    </a-radio-group>
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="80" v-if="formData.httpHandlerOptions">
                <a-col :span="6">
                  <a-form-item :label="$t('route.http.handler.max.connections.per.server.label')">
                    <a-input-number v-model="formData.httpHandlerOptions.maxConnectionsPerServer" :placeholder="$t('route.http.handler.max.connections.per.server.placeholder')" :hide-button="true" />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane key="6" :title="$t('route.loadbalancer.title')">
              <a-row :gutter="80" v-if="formData.loadBalancerOptions">
                <a-col :span="6">
                  <a-form-item :label="$t('route.loadbalancer.type.label')" field="loadbalancerType">
                    <a-select v-model="formData.loadBalancerOptions.type" allow-clear :placeholder="$t('route.loadbalancer.type.placeholder')">
                      <a-option value="LeastConnection">{{ $t('route.loadbalancer.type.least.connection.label') }}</a-option>
                      <a-option value="RoundRobin">{{ $t('route.loadbalancer.type.round.robin.label') }}</a-option>
                      <a-option value="NoLoadBalance">{{ $t('route.loadbalancer.type.no.load.balance.label') }}</a-option>
                      <a-option value="CookieStickySessions">{{ $t('route.loadbalancer.type.cookie.sticky.sessions.label') }}</a-option>
                    </a-select>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.loadbalancer.key.label')" field="loadbalancerKey">
                    <a-input v-model="formData.loadBalancerOptions.key" :placeholder="$t('route.loadbalancer.key.placeholder')"></a-input>
                  </a-form-item>
                </a-col>
                <a-col :span="6">
                  <a-form-item :label="$t('route.loadbalancer.expiry.label')" field="loadbalancerExpiry">
                    <a-input-number v-model="formData.loadBalancerOptions.expiry" :hide-button="true" :placeholder="$t('route.loadbalancer.expiry.placeholder')"></a-input-number>
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
          </a-tabs>
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
    <a-modal v-model:visible="visible" :title="$t(`${isCreate ? 'route.downstreamHostAndPorts.create.modal' : 'route.downstreamHostAndPorts.update.modal'}`)" :mask-closable="false" @cancel="handleCancel" @ok="handleOk">
      <a-form :model="hostAndPortForm">
        <a-form-item field="host" :label="$t('route.downstreamHostAndPorts.host.table')">
          <a-input v-model="hostAndPortForm.host" />
        </a-form-item>
        <a-form-item field="port" :label="$t('route.downstreamHostAndPorts.port.table')">
          <a-input-number v-model="hostAndPortForm.port" />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, nextTick } from 'vue'
import { useI18n } from 'vue-i18n'
import { FormInstance } from '@arco-design/web-vue/es/form'
import useLoading from '@/hooks/loading'
import { getRoute, addRoute, updateRoute } from '@/api/route'
import { RouteModel } from '@/models/route'
import { StatusEnum } from '@/models/common'
import { AuthenticationOptions, LoadBalancerOptions, HttpHandlerOptions, QoSOptions, RateLimitOptions, ServiceDiscoveryProviderOptions, DownstreamHostAndPortOptions, FileCacheOptions } from '@/models/ocelot-options'
import { Message } from '@arco-design/web-vue'
import { useRouteStore } from '@/store'

const { t } = useI18n()
const routeStore = useRouteStore()
const visible = ref(false)
const isCreate = ref(true)

const projectId = computed(() => routeStore.getProjectIdCache)
const routeId = computed(() => routeStore.getRouteIdCache)
const hostAndPortForm = ref<DownstreamHostAndPortOptions>({
  host: '',
  port: 0,
  routeId: 0
})

const formData = ref<RouteModel>({
  id: routeId.value,
  projectId: projectId.value,
  downstreamPathTemplate: '/{url}',
  upstreamPathTemplate: '/undefined/{url}',
  upstreamHttpMethod: ['Get', 'Post', 'Put', 'Patch', 'Delete', 'Option'],
  downstreamHttpMethod: undefined,
  downstreamHttpVersion: '1.1',
  upstreamHost: undefined,
  requestIdKey: undefined,
  routeIsCaseSensitive: false, // 开启上下游路由模板大小写匹配
  useServiceDiscovery: false, // 是否使用服务发现
  serviceName: undefined, // 服务名
  serviceNamespace: undefined, // 如果您的下游服务位于不同的名称空间中，您可以通过指定ServiceNamespace来覆盖Route级别的全局设置
  downstreamScheme: undefined, //下游请求的协议，如：http,htttps
  downstreamHostAndPorts: [],
  qoSOptions: new QoSOptions(),
  loadBalancerOptions: new LoadBalancerOptions(),
  rateLimitOptions: new RateLimitOptions(),
  authenticationOptions: new AuthenticationOptions(),
  dangerousAcceptAnyServerCertificateValidator: false, // 如果要忽略SSL警告/错误，请设置为true
  httpHandlerOptions: new HttpHandlerOptions(), // HttpHandler配置
  delegatingHandlers: [], //委托处理程序
  fileCacheOptions: new FileCacheOptions(),
  serviceDiscoveryProviderOptions: new ServiceDiscoveryProviderOptions(),
  priority: undefined,
  sort: undefined,
  status: StatusEnum.Enable
})

const formRef = ref<FormInstance>()
const handleCancel = () => {
  visible.value = false
}
const handleOk = async () => {
  visible.value = false
  if (isCreate.value) {
    formData.value.downstreamHostAndPorts?.push(hostAndPortForm.value)
  }
  const msg = t('submit.success')
  Message.success({
    content: msg,
    duration: 5 * 1000
  })
}
const handleDelete = (deleteRouteHostPortParam: DownstreamHostAndPortOptions) => {
  formData.value.downstreamHostAndPorts = formData.value.downstreamHostAndPorts?.filter((obj) => obj !== deleteRouteHostPortParam)
}

// 添加或编辑下游主机端口
const handleEdit = (isAdd: boolean, editHostPortParams: DownstreamHostAndPortOptions | undefined) => {
  if (isAdd) {
    hostAndPortForm.value = {
      host: '',
      port: 0,
      routeId: routeId.value
    }
  } else {
    hostAndPortForm.value = editHostPortParams as DownstreamHostAndPortOptions
  }
  visible.value = true
  isCreate.value = isAdd
}
const { loading, setLoading } = useLoading()
const onSubmitClick = async () => {
  try {
    setLoading(true)
    if (formData.value.id !== undefined && formData.value.id > 0) {
      const { data } = await updateRoute(formData.value)
      formData.value.id = data
      routeStore.toggleRouteId(data)
    } else {
      const { data } = await addRoute(formData.value)
      formData.value.id = data
      routeStore.toggleRouteId(data)
    }
    const msg = t('submit.success')
    Message.success({
      content: msg,
      duration: 5 * 1000
    })
    setLoading(false)
  } finally {
    setLoading(false)
  }
}
const fetchData = async () => {
  try {
    if (routeId.value > 0) {
      const { data } = await getRoute(routeId.value)
      formData.value = data
    }
  } catch (err) {
    // you can report use errorHandler or other
  }
}
fetchData()

/** ************************
 * 授权作用域新增编辑
 *********************** */
const inputScopeRef = ref()
const showScopeInput = ref(false)
const inputScopeVal = ref('')

const handleEditScopeTag = () => {
  showScopeInput.value = true

  nextTick(() => {
    if (inputScopeRef.value) {
      inputScopeRef.value.focus()
    }
  })
}

const handleAddScopeTag = () => {
  if (inputScopeVal.value) {
    if (formData.value.authenticationOptions === undefined) formData.value.authenticationOptions = new AuthenticationOptions()

    if (formData.value.authenticationOptions.AllowedScopes === undefined) {
      formData.value.authenticationOptions.AllowedScopes = [inputScopeVal.value]
    } else {
      formData.value.authenticationOptions.AllowedScopes.push(inputScopeVal.value)
    }
    inputScopeVal.value = ''
  }
  showScopeInput.value = false
}

const handleRemoveScopeTag = (key: string) => {
  if (formData.value.authenticationOptions === undefined) return
  if (formData.value.authenticationOptions.AllowedScopes === undefined) return
  formData.value.authenticationOptions.AllowedScopes = formData.value.authenticationOptions.AllowedScopes.filter((tag) => tag !== key)
}

/** ************************
 * 限流白名单新增编辑
 *********************** */
const inputWhiteListRef = ref()
const showWhiteListInput = ref(false)
const inputWhiteListVal = ref('')

const handleEditWhiteListTag = () => {
  showWhiteListInput.value = true

  nextTick(() => {
    if (inputWhiteListRef.value) {
      inputWhiteListRef.value.focus()
    }
  })
}

const handleAddWhiteListTag = () => {
  if (inputWhiteListVal.value) {
    if (formData.value.rateLimitOptions === undefined) formData.value.rateLimitOptions = new RateLimitOptions()
    if (formData.value.rateLimitOptions.clientWhitelist == undefined) {
      formData.value.rateLimitOptions.clientWhitelist = [inputWhiteListVal.value]
    } else {
      formData.value.rateLimitOptions.clientWhitelist.push(inputWhiteListVal.value)
    }
    inputWhiteListVal.value = ''
  }
  showWhiteListInput.value = false
}

const handleRemoveWhiteListTag = (key: string) => {
  if (formData.value.rateLimitOptions === undefined) return
  if (formData.value.rateLimitOptions.clientWhitelist == undefined) return
  formData.value.rateLimitOptions.clientWhitelist = formData.value.rateLimitOptions.clientWhitelist.filter((tag) => tag !== key)
}

/** ************************
 * 委托处理新增编辑
 *********************** */
const inputDelegatingHandlersRef = ref()
const showDelegatingHandlersInput = ref(false)
const inputDelegatingHandlersVal = ref('')

const handleEditDelegatingHandlersTag = () => {
  showDelegatingHandlersInput.value = true

  nextTick(() => {
    if (inputDelegatingHandlersRef.value) {
      inputDelegatingHandlersRef.value.focus()
    }
  })
}

const handleAddDelegatingHandlersTag = () => {
  if (inputDelegatingHandlersVal.value) {
    formData.value.delegatingHandlers.push(inputDelegatingHandlersVal.value)
    inputDelegatingHandlersVal.value = ''
  }
  showDelegatingHandlersInput.value = false
}

const handleRemoveDelegatingHandlersTag = (key: string) => {
  formData.value.delegatingHandlers = formData.value.delegatingHandlers.filter((tag) => tag !== key)
}
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
