# Smart Plant Care - SaaS Platform

A comprehensive plant care management platform designed for gardeners, homeowners, and plant enthusiasts. This application provides inventory management, care scheduling, and plant health tracking with a subscription-based SaaS model.

## Features

### 🔐 Authentication & User Management

- **JWT-based authentication** with secure token management
- **User registration and login** with email/password
- **Subscription tier management** (Free, Basic, Premium)
- **Plant limits** based on subscription tier
- **Secure API endpoints** with user isolation

### 🌿 Plant Management

- **Add, edit, and delete plants** with comprehensive details
- **Plant species tracking** and care instructions
- **Watering and fertilizing schedules** with automatic reminders
- **Plant location and sunlight requirements**
- **Care history tracking** and notes
- **Image support** for plant identification

### 📊 Smart Care Features

- **Automated watering reminders** based on plant type and conditions
- **Fertilizing schedules** with frequency tracking
- **Plant health monitoring** and care logs
- **Subscription-based plant limits** (Free: 5 plants, Basic: 25 plants, Premium: Unlimited)

## Architecture

### Backend (.NET 9.0 Web API)

- **ASP.NET Core 9.0** with MongoDB integration
- **JWT Authentication** using Microsoft.AspNetCore.Authentication.JwtBearer
- **Password hashing** with BCrypt.Net-Next
- **User isolation** - each user can only access their own plants
- **Subscription validation** and plant limit enforcement

### Frontend (React 19.1.0)

- **Modern React** with hooks and functional components
- **JWT token management** for authenticated requests
- **Responsive design** for mobile and desktop use
- **Real-time updates** and state management

### Database (MongoDB)

- **User collections** with authentication data
- **Plant collections** with user ownership
- **Scalable document storage** for flexible plant data

## API Endpoints

### Authentication

- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `GET /api/auth/profile` - Get user profile (authenticated)
- `POST /api/auth/refresh` - Refresh JWT token

### Plants (All require authentication)

- `GET /api/plant` - Get user's plants
- `GET /api/plant/{id}` - Get specific plant
- `POST /api/plant` - Create new plant
- `PUT /api/plant/{id}` - Update plant
- `DELETE /api/plant/{id}` - Delete plant (soft delete)
- `POST /api/plant/{id}/water` - Mark plant as watered
- `POST /api/plant/{id}/fertilize` - Mark plant as fertilized
- `GET /api/plant/needing-water` - Get plants needing water
- `GET /api/plant/needing-fertilizer` - Get plants needing fertilizer

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- MongoDB instance (local or cloud)
- Node.js 18+ (for frontend)

### Backend Setup

1. **Clone the repository**
2. **Update appsettings.json** with your MongoDB connection string
3. **Generate a secure JWT secret key** for production
4. **Run the API**:
   ```bash
   cd SmartPlantCareApi
   dotnet restore
   dotnet run
   ```

### Frontend Setup

1. **Navigate to client directory**
2. **Install dependencies**:
   ```bash
   npm install
   ```
3. **Start the development server**:
   ```bash
   npm start
   ```

### Environment Configuration

Update `appsettings.json` with your settings:

```json
{
  "PlantDatabaseSettings": {
    "ConnectionString": "your-mongodb-connection-string",
    "DatabaseName": "SmartPlantCareDb",
    "PlantCollectionName": "Plants"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secure-jwt-secret-key",
    "Issuer": "SmartPlantCareApi",
    "Audience": "SmartPlantCareApp",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

## Security Features

- **JWT token validation** with configurable expiration
- **Password hashing** using BCrypt
- **User isolation** - users can only access their own data
- **CORS configuration** for secure cross-origin requests
- **Input validation** and sanitization
- **Soft delete** for data preservation

## Subscription Tiers

### Free Tier

- **5 plants maximum**
- Basic plant care features
- Watering and fertilizing reminders

### Basic Tier ($4.99/month)

- **25 plants maximum**
- Advanced care scheduling
- Plant health analytics
- Priority support

### Premium Tier ($9.99/month)

- **Unlimited plants**
- Advanced features
- API access
- Premium support
- Custom care schedules

## Deployment

### Production Considerations

1. **Use strong JWT secret keys** (256+ bits)
2. **Enable HTTPS** for all communications
3. **Configure proper CORS** for your domain
4. **Set up MongoDB authentication** and network security
5. **Use environment variables** for sensitive configuration
6. **Implement rate limiting** for API endpoints
7. **Set up monitoring** and logging

### Docker Support

The application can be containerized for easy deployment:

```bash
# Build and run with Docker
docker build -t smart-plant-care .
docker run -p 5000:5000 smart-plant-care
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions:

- Create an issue in the repository
- Contact the development team
- Check the documentation and API reference

---

**Built with ❤️ for plant lovers everywhere**
