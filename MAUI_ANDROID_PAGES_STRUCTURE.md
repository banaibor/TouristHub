# MAUI Android Project Structure and Navigation

## Current Configuration

### ? **Fixed: MainPage.xaml Root Component**
The MAUI Android app is now properly configured to use its **own Pages** from the MAUI project, not shared UI from the Web App.

**MainPage.xaml** now loads:
```xml
<RootComponent Selector="#app" ComponentType="{x:Type local:Components.Routes}" />
```

## MAUI Project Pages Structure

The MAUI project contains its own complete set of pages located in `MAUI/Components/Pages/`:

### ?? **Entry Points**
- **`/`** ? `Index.razor` - Welcome page with navigation options
- **`/diagnostic`** ? `Diagnostic.razor` - System diagnostic and testing

### ?? **Authentication Pages**
- **`/login`** ? `Login/Login.razor` - MAUI-specific login page
- **`/register`** ? `Login/Register.razor` - MAUI-specific registration page

### ?? **Dashboard Pages (Role-based)**
- **`/admin/dashboards/dashboard`** ? `Admin/Dashboards/Dashboard.razor`
- **`/manager/dashboards/dashboard`** ? `Manager/Dashboards/Dashboard.razor`
- **`/user/dashboards/dashboard`** ? `User/Dashboards/Dashboard.razor`

### ?? **Guide Search Features**
- **`/user/guides/search`** ? `User/GuidesSearch/Search.razor`
- **`/user/guides/profile`** ? `User/GuidesSearch/Profile.razor`

## Layout Structure

### ?? **MAUI-Specific Layout**
- **MainLayout** (`Components/Layout/MainLayout.razor`) - MAUI-optimized layout
- **NavMenu** (`Components/Layout/NavMenu.razor`) - Mobile-friendly navigation

### ?? **Navigation Features**
- Responsive sidebar navigation
- Conditional navigation (hides on login/register pages)
- Mobile-optimized navigation menu
- Bootstrap Icons integration

## What Happens When Loading Android App

### 1. **App Startup Sequence**
```
MainActivity.OnCreate() ? 
MauiProgram.CreateMauiApp() ? 
MainPage (BlazorWebView) ? 
Routes Component ? 
Index Page (/)
```

### 2. **Initial Navigation Flow**
1. **Loading Screen** ? Shows "Loading TouristHub..." 
2. **Index Page** ? Welcome page with platform detection
3. **Navigation Options**:
   - **Diagnostic** ? System testing and backend connectivity
   - **Login** ? Authentication flow

### 3. **After Login Success**
Based on user role, navigates to:
- **Admin** ? `/admin/dashboards/dashboard`
- **Manager** ? `/manager/dashboards/dashboard`
- **User** ? `/user/dashboards/dashboard`

## Key Features

### ? **MAUI-Specific Implementation**
- ? Uses MAUI project's own Pages (not Web App shared UI)
- ? Mobile-optimized layouts and navigation
- ? Platform-specific styling and components
- ? Proper error boundaries and error handling
- ? Backend connectivity with Android emulator support

### ?? **Enhanced User Experience**
- ? Responsive design for mobile devices
- ? Platform detection (shows "Android" on Android devices)
- ? Smooth navigation with proper routing
- ? Role-based dashboard access
- ? Diagnostic tools for troubleshooting

### ?? **Development Features**
- ? Hot reload support
- ? WebView debugging enabled in debug mode
- ? Comprehensive logging and error handling
- ? Network security configuration for development

## Testing the Android App

### 1. **Expected Startup Flow**
```
?? Loading Screen ? 
?? Index/Welcome Page ? 
?? Login Page ? 
?? Role-specific Dashboard
```

### 2. **Navigation Testing**
- Test navigation between pages using NavMenu
- Verify role-based dashboard access
- Test guide search functionality
- Verify responsive layout on different screen sizes

### 3. **Backend Connectivity**
- Login should connect to API at `http://10.0.2.2:5500/`
- Backend connection test available on Diagnostic page
- All API calls use Android emulator-specific URLs

## Summary

The MAUI Android app now properly uses its own Pages and components, providing a native mobile experience separate from the Web App. The routing is configured to show the Index page first, allowing users to navigate to Login and then to role-specific dashboards.