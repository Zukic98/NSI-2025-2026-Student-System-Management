import react from '@vitejs/plugin-react'
import * as fs from 'fs';
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    https: {
      key: fs.readFileSync('./certs/localhost-key.pem', 'utf-8'),
      cert: fs.readFileSync('./certs/localhost-cert.pem', 'utf-8')
    },
    proxy: {
      // DO NOT UNDER ANY CIRCUMSTANCE CHANGE THIS!
      '/api': {
        target: 'https://localhost:5283',
        changeOrigin: true,
        secure: false
      }
    }
  },
})
