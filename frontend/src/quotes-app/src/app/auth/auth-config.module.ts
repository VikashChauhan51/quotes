import { NgModule } from '@angular/core';
import { AuthModule,LogLevel } from 'angular-auth-oidc-client';


@NgModule({
    imports: [AuthModule.forRoot({
        config: {
            authority: 'https://localhost:5001',
            redirectUrl: window.location.origin,
            postLogoutRedirectUri: window.location.origin,
            clientId: 'quoteclient',
            scope: 'openid profile email roles offline_access', 
            responseType: 'code',
            silentRenew: true,
            useRefreshToken: true,
            renewTimeBeforeTokenExpiresInSeconds: 10,
            logLevel: LogLevel.Debug,
        }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
