#region Copyright (c) 2023 [SharpNinjaArtLibrary]. All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace SharpNinjaArtLibrary;

public static class SharpNinjaArtTask
{
    //https://steven-giesel.com/blogPost/d38e70b4-6f36-41ff-8011-b0b0d1f54f6e 참고
    public static void FireAndForget(this Task task, Action<Exception>? handler = null) =>
        task.ContinueWith(t =>
            handler?.Invoke(t.Exception!), TaskContinuationOptions.OnlyOnFaulted);
    public static async Task<TResult> Retry<TResult>(this Func<Task<TResult>> taskFactory, int maxRetries, TimeSpan delay)
    {
        for (var i = 0; i < maxRetries; i++)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch
            {
                if (i == maxRetries - 1)
                    throw;

                await Task.Delay(delay).ConfigureAwait(false);
            }
        }

        return default!;
    }
    public static async Task OnFailure(this Task task, Action<Exception> onFailure)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onFailure(ex);
        }
    }
    public static async Task WithTimeout(this Task task, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource();
        var delayTask = Task.Delay(timeout, cts.Token);
        var completedTask = await Task.WhenAny(task, delayTask);
        if (completedTask == delayTask)
            throw new TimeoutException();

        cts.Cancel();
        await task;
        /*
          await task;는 task의 완료를 기다리고, task가 성공적으로 완료되었는지, 실패했는지를 결정하는 데 필요합니다. 
          또한, task가 예외를 발생시킨 경우 await task;를 통해 해당 예외를 재발생시킬 수 있습니다. 
          이를 통해 비동기 작업의 오류를 호출자에게 전달하고, 이를 적절히 처리할 수 있게 해줍니다. 
        */
        //NET6 이상부터는 task.WaitAsync 사용가능!
    }
    public static async Task<TResult> Fallback<TResult>(this Task<TResult> task, TResult fallbackValue)
    {
        try
        {
            return await task;
        }
        catch
        {
            return fallbackValue;
        }
    }
}