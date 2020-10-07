import {environment} from '../../environments/environment';

export const DomainName = environment.production ? '' : 'https://localhost:44317';