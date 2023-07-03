module.exports = {
  printWidth: 300, // 每行文字个数超出此限制将会被迫换行
  tabWidth: 2, // 超过最大值换行
  semi: false, // 行尾是否使用分号，默认为true
  singleQuote: true, //字符串是否使用单引号，默认为false，使用双引号
  htmlWhitespaceSensitivity: 'ignore',
  vueIndentScriptAndStyle:false, // 是否缩进Vue文件中<script>和<style>标签内的代码,true:缩进Vue文件中的脚本和样式标记
  trailingComma: 'none', // 函数后面不加逗号，如果不写这一个，在methods 最后一个函数也会加逗号，eslint会报错，多了一个逗号
}