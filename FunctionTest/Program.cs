using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCL.Net;
using Cloo;

namespace FunctionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //One();
            //Two();
            //Three();
            //Four();
            Five();
        }

        static void One()
        {
            ErrorCode eError;
            uint iNum;
            // Get the platform and device information
            Platform[] platforms = Cl.GetPlatformIDs(out eError);
            Device[] devices = Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out eError);

            // Create the context and command queue
            Context context = Cl.CreateContext(null, 1, devices, null, IntPtr.Zero, out ErrorCode error);
            CommandQueue commandQueue = Cl.CreateCommandQueue(context, devices[0], CommandQueueProperties.None, out error);

            // Load the EDID data from file
            byte[] edidData = File.ReadAllBytes("edid.bin");

            // Create the buffer
            Mem edidBuffer = (Mem)Cl.CreateBuffer(context, MemFlags.ReadWrite | MemFlags.CopyHostPtr, edidData.Length, out eError);

            // Get the kernel source code
            string kernelSource = @"
                __kernel void write_edid(__global uchar* edid_data, uint edid_size)
                {
                    // Write the EDID data to the display
                    // ...
                }";

            // Compile the kernel
            OpenCL.Net.Program program = Cl.CreateProgramWithSource(context, 1, new[] { kernelSource }, null, out error);
            Cl.ProgramNotify eP = null;
            Cl.BuildProgram(program, 1, devices, null, eP, IntPtr.Zero);

            // Create the kernel
            Kernel kernel = Cl.CreateKernel(program, "write_edid", out error);

            // Set the arguments
            Cl.SetKernelArg(kernel, 0, edidBuffer);
            Cl.SetKernelArg(kernel, 1, (uint)edidData.Length);

            // Execute the kernel
            Event ev;
            Cl.EnqueueNDRangeKernel(commandQueue, kernel, 1, null, new[] { (IntPtr)edidData.Length }, null, 0, null, out ev);

            // Wait for the kernel to finish
            Cl.Finish(commandQueue);

            // Read the results back from the buffer
            byte[] result = new byte[edidData.Length];
            OpenCL.Net.InfoBuffer ptr = Cl.EnqueueMapBuffer(commandQueue, edidBuffer, OpenCL.Net.Bool.True, MapFlags.Read, IntPtr.Zero, (IntPtr)edidData.Length, 0, null, out ev, out error);
            //Cl.EnqueueUnmapMemObject(commandQueue, edidBuffer, ptr, 0, null, out ev);
            Cl.Finish(commandQueue);

            // Clean up
            Cl.ReleaseMemObject(edidBuffer);
            Cl.ReleaseKernel(kernel);
            Cl.ReleaseProgram(program);
            Cl.ReleaseCommandQueue(commandQueue);
            Cl.ReleaseContext(context);
        }

        static void Two()
        {
            // Load EDID data from file
            byte[] edidData = File.ReadAllBytes("myedid.bin");

            // Create an OpenCL context for a GPU device
            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Gpu, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, IntPtr.Zero);

            // Compile the kernel code
            string kernelSource = File.ReadAllText("ReadWriteEDID.cl");
            ComputeProgram program = new ComputeProgram(context, kernelSource);
            program.Build(null, null, null, IntPtr.Zero);

            // Create a kernel object
            ComputeKernel kernel = program.CreateKernel("ReadWriteEDID");

            // Create a buffer object to store the EDID data on the OpenCL device
            ComputeBuffer<byte> edidBuffer = new ComputeBuffer<byte>(context, ComputeMemoryFlags.WriteOnly, edidData.Length);

            // Set the kernel arguments
            kernel.SetMemoryArgument(0, edidBuffer);
            kernel.SetValueArgument<int>(1, edidData.Length);

            // Create a command queue for the GPU device
            ComputeCommandQueue queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            // Execute the kernel
            queue.Execute(kernel, null, new long[] { edidData.Length }, null, null);

            // Read the EDID data from the OpenCL device buffer
            queue.ReadFromBuffer(edidBuffer, ref edidData, true, null);

            // Save the modified EDID data to a file
            File.WriteAllBytes("modified_edid.bin", edidData);

            Console.WriteLine("EDID data has been modified and saved to 'modified_edid.bin'. Press any key to exit.");
            Console.ReadKey();
        }

        static void Three()
        {
            // Set up OpenCL
            var platform = ComputePlatform.Platforms[0];
            var context = new ComputeContext(
                ComputeDeviceTypes.Gpu, new ComputeContextPropertyList(platform), null, IntPtr.Zero);
            var queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            // Load the EDID reader kernel
            var kernelSource = System.IO.File.ReadAllText("ReadWriteEDID.cl");
            var program = new ComputeProgram(context, kernelSource);
            program.Build(null, null, null, IntPtr.Zero);

            // Allocate input and output buffers
            var inputBuffer = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly, 128);
            var outputBuffer = new ComputeBuffer<byte>(context, ComputeMemoryFlags.WriteOnly, 256);

            // Set the input data
            byte[] input = new byte[128];
            // TODO: Set the input data to the EDID you want to read
            //inputBuffer.WriteToBuffer(input, true, null);

            // Set the kernel arguments
            var kernel = program.CreateKernel("ReadEDID");
            kernel.SetMemoryArgument(0, inputBuffer);
            kernel.SetMemoryArgument(1, outputBuffer);

            // Execute the kernel
            queue.Execute(kernel, null, new long[] { 256 }, null, null);
            queue.Finish();

            // Read the output data
            byte[] output = new byte[256];
            //outputBuffer.ReadFromBuffer(output, true, null);

            // TODO: Parse the output data to get the EDID information
            // The first 128 bytes are the raw EDID data, and the next 128 bytes are the EDID extension data.
            // You can use a library like EDIDLib to parse the EDID data.

            // Clean up
            inputBuffer.Dispose();
            outputBuffer.Dispose();
            kernel.Dispose();
            program.Dispose();
            queue.Dispose();
            context.Dispose();
        }

        static void Four()
        {
            // OpenCL platform and device selection
            ComputePlatform platform = ComputePlatform.Platforms[0];
            ComputeDevice device = platform.Devices[0];
            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Gpu, new ComputeContextPropertyList(ComputePlatform.Platforms[0]), null, IntPtr.Zero);
            // Map the EDID physical address to OpenCL memory
            IntPtr edidAddress = new IntPtr(0xA0); // replace with your EDID physical address
            ComputeBuffer<byte> edidBuffer = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, 128, edidAddress);

            // Load the kernel source from a file
            string kernelSource = File.ReadAllText("ReadWriteEDID.cl");

            // Build the kernel
            ComputeProgram program = new ComputeProgram(context, kernelSource);
            program.Build(new[] { device }, null, null, IntPtr.Zero);
            ComputeKernel kernel = program.CreateKernel("read_write_edid");

            // Create a command queue for the device
            ComputeCommandQueue queue = new ComputeCommandQueue(context, device, ComputeCommandQueueFlags.None);

            // Set the kernel arguments
            kernel.SetMemoryArgument(0, edidBuffer);

            // Define the work size and execute the kernel
            ComputeEventList eventList = new ComputeEventList();
            queue.Execute(kernel, null, new long[] { 1 }, null, eventList);

            // Wait for the kernel to finish and read the EDID data
            queue.Finish();
            byte[] edidData = new byte[128];
            //edidBuffer.ReadFromBuffer(queue, ref edidData, true, eventList);
    
            // Display the EDID data
            Console.WriteLine(BitConverter.ToString(edidData));
        }

        static void Five()
        {
            double dReust = Math.Asin(0.0005);
            double dDegrees = (dReust * 180 / Math.PI);
        }
    }
}
