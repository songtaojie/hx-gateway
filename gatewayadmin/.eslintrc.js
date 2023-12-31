module.exports = {
  root: true,
  parser: 'vue-eslint-parser',
  parserOptions: {
    // Parser that checks the content of the <script> tag
    parser: '@typescript-eslint/parser',
    sourceType: 'module',
    ecmaVersion: 2020,
    ecmaFeatures: {
      jsx: true,
    },
  },
  env: {
    'browser': true,
    'node': true,
    "es6": true,
    // The Follow config only works with eslint-plugin-vue 	        v8.0.0+
    "vue/setup-compiler-macros": true,
  },
  plugins: ['@typescript-eslint'],
  extends: [
    // Airbnb JavaScript Style Guide https://github.com/airbnb/javascript
    'plugin:@typescript-eslint/recommended',
    'plugin:vue/vue3-recommended',
    'plugin:prettier/recommended',
  ],
  rules: {
	'@typescript-eslint/ban-ts-ignore': 'off',
	'@typescript-eslint/explicit-function-return-type': 'off',
	'@typescript-eslint/no-explicit-any': 'off',
	'@typescript-eslint/no-var-requires': 'off',
	'@typescript-eslint/no-empty-function': 'off',
	'@typescript-eslint/no-use-before-define': 'off',
	'@typescript-eslint/ban-ts-comment': 'off',
	'@typescript-eslint/ban-types': 'off',
	'@typescript-eslint/no-non-null-assertion': 'off',
	'@typescript-eslint/explicit-module-boundary-types': 'off',
	'@typescript-eslint/no-redeclare': 'error',
	'@typescript-eslint/no-non-null-asserted-optional-chain': 'off',
	// '@typescript-eslint/no-unused-vars': [2],
	'vue/custom-event-name-casing': 'off',
	'vue/attributes-order': 'off',
	'vue/one-component-per-file': 'off',
	'vue/html-closing-bracket-newline': 'off',
	'vue/max-attributes-per-line': 'off',
	'vue/multiline-html-element-content-newline': 'off',
	'vue/singleline-html-element-content-newline': 'off',
	'vue/attribute-hyphenation': 'off',
	'vue/html-self-closing': 'off',
	'vue/no-multiple-template-root': 'off',
	'vue/require-default-prop': 'off',
	'vue/no-v-model-argument': 'off',
	'vue/no-arrow-functions-in-watch': 'off',
	'vue/no-template-key': 'off',
	'vue/no-v-html': 'off',
	'vue/comment-directive': 'off',
	'vue/no-parsing-error': 'off',
	'vue/no-deprecated-v-on-native-modifier': 'off',
	'vue/multi-word-component-names': 'off',
	'no-useless-escape': 'off',
	'no-sparse-arrays': 'off',
	'no-prototype-builtins': 'off',
	'no-constant-condition': 'off',
	'no-use-before-define': 'off',
	'no-restricted-globals': 'off',
	'no-restricted-syntax': 'off',
	'generator-star-spacing': 'off',
	'no-unreachable': 'off',
	'no-multiple-template-root': 'off',
	'no-unused-vars': 'off',
	'no-v-model-argument': 'off',
	'no-case-declarations': 'off',
	'no-console': 'off',
	'no-redeclare': 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 2 : 0,
    'no-param-reassign': 0,
	// 取消对LF和CRLF的校验
	'linebreak-style':[
		0,
		'error',
		'window'
	]
  },
};
