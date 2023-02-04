import { NgModule } from '@angular/core';
import { AuthModule, LogLevel  } from 'angular-auth-oidc-client';


@NgModule({
    imports: [AuthModule.forRoot({
        config: {
            authority: 'https://localhost:5001',
            redirectUrl: `https://localhost:4200/signin-callback`,
            postLogoutRedirectUri: `https://localhost:4200/signout-callback`,
            clientId: 'please-enter-clientId',
            scope: 'openid profile email offline_access', // 'openid profile ' + your scopes
            responseType: 'code',
            silentRenew: false,
            useRefreshToken: true,
           logLevel: LogLevel.Debug,

        }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
