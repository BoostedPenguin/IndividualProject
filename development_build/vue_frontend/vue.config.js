module.exports = {
    devServer: {
        port: 3000
    },
    configureWebpack: {
        optimization: {
            splitChunks: {
                chunks: 'all'
            }
        }
    }
}