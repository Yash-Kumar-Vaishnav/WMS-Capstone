export const environment = {
  production: window.location.hostname !== 'localhost',
  apiUrl: window.location.hostname === 'localhost' 
    ? 'http://localhost:5176/api' 
    : 'https://wms-api-v2-g2a7dzb0gfdxgyaj.centralindia-01.azurewebsites.net/api'
};
