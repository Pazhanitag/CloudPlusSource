// http://eslint.org/docs/user-guide/configuring

module.exports = {
  root: true,
  parser: 'babel-eslint',
  parserOptions: {
    sourceType: 'module'
  },
  env: {
    browser: true,
  },
  extends: 'airbnb-base',
  // required to lint *.vue files
  plugins: [
    'html'
  ],
  // check if imports actually resolve
  'settings': {
    'import/resolver': {
      'webpack': {
        'config': 'build/webpack.base.conf.js'
      }
    }
  },
  // add your custom rules here
  'rules': {
    'linebreak-style': ['error', 'windows'],
    'arrow-parens': ['error', 'as-needed'],
    // don't require .vue extension when importing
    'import/extensions': ['error', 'always', {
      'js': 'never',
      'vue': 'never'
    }],
    // allow optionalDependencies
    'import/no-extraneous-dependencies': ['error', {
      'optionalDependencies': ['test/unit/index.js']
    }],
    // allow debugger during development
    'no-debugger': process.env.NODE_ENV === 'production' ? 2 : 0,
    // allow reassignement of function parameters for state parameters in mutations
    'no-param-reassign': ["error", { "props": true, "ignorePropertyModificationsFor": ["userState", "portalState"] }],
    // allow to import all defined styled components in main.js without using the variable afterwards
    'no-unused-vars': ["error", { "varsIgnorePattern": "[styledshared]Components" }],
    'no-param-reassign': ["error", { "props": false }],
    'no-console': ["error", { allow: ["log"] }],
    'no-underscore-dangle': [1, { "allow": ["_chart", "_datasetIndex", "_index"] }]
  }
}
