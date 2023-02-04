import { NgModule } from '@angular/core';
import { AuthModule, LogLevel  } from 'angular-auth-oidc-client';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [AuthModule.forRoot({
        config: {
            authority: 'https://localhost:5001',
            redirectUrl: `${window.location.origin}/signin-callback`,
            postLogoutRedirectUri: `${window.location.origin}/signout-callback`,
            clientId: 'please-enter-clientId',
            scope: 'openid profile email offline_access', // 'openid profile ' + your scopes
            responseType: 'code',
            silentRenew: true,
            silentRenewUrl: `${window.location.origin}/silent-renew.html`,
            renewTimeBeforeTokenExpiresInSeconds: 10,
            useRefreshToken: true,
           logLevel: LogLevel.Debug,

        }
      }),
      HttpClientModule,
     CommonModule,
     RouterModule],
    exports: [AuthModule],
})
export class AuthConfigModule {}
