import react from '@vitejs/plugin-react';
import * as child_process from 'child_process';
import * as fs from 'fs';
import * as path from 'path';
import { defineConfig } from 'vite';

// https://vite.dev/config/
export default defineConfig(({ command }) => {
    // -------------------------------------------------------------
    // VERCEL PART (PRODUCTION BUILD)
    // If the build command is executed, simply return the basic config
    // and skip the certificate generation process completely.
    // -------------------------------------------------------------
    if (command === 'build') {
        return {
            plugins: [react()],
        };
    }

    // -------------------------------------------------------------
    // LOCAL DEVELOPMENT PART (LOCAL DEV)
    // This part runs only on your local machine when you run 'npm run dev'.
    // -------------------------------------------------------------
    const env = process.env;

    const baseFolder =
        env.APPDATA && env.APPDATA !== ''
            ? `${env.APPDATA}/ASP.NET/https`
            : `${env.HOME}/.aspnet/https`;

    fs.mkdirSync(baseFolder, { recursive: true });

    const certificateName = 'UNSA_SMS';
    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        // We use try-catch to prevent crashing the app if 'dotnet' is not installed/found
        try {
            const result = child_process.spawnSync(
                'dotnet',
                [
                    'dev-certs',
                    'https',
                    '--export-path',
                    certFilePath,
                    '--format',
                    'Pem',
                    '--no-password',
                ],
                { stdio: 'inherit' }
            );

            if (result.status !== 0) {
                console.error('Warning: Could not create HTTPS certificate via dotnet.');
            } else {
                console.info('Created certificate/key pair.');
            }
        } catch (e) {
            console.error('Warning: dotnet command not found, skipping cert generation.');
        }
    } else {
        console.info('Detected already present certificate/key pair.');
    }

    return {
        plugins: [react()],
        server: {
            https: {
                key: fs.readFileSync(keyFilePath, 'utf-8'),
                cert: fs.readFileSync(certFilePath, 'utf-8'),
            },
            proxy: {
                // DO NOT UNDER ANY CIRCUMSTANCE CHANGE THIS!
                '/api': {
                    target: 'https://localhost:5283',
                    changeOrigin: true,
                    secure: false,
                },
            },
        },
    };
});