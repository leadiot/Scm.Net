using Com.Scm.Mqtt;
using Com.Scm.Mqtt.Impl;
using Com.Scm.Utils;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace WinForm
{
    public partial class Form1 : Form
    {
        private IMqttPublisher _mqttPublisher;
        private IMqttSubscriber _mqttSubscriber;
        private Random _random;
        private double _baseTemperature;
        private bool _isRunning;

        public Form1()
        {
            InitializeComponent();

            _random = new Random();
            _baseTemperature = 25.0;
            _isRunning = false;
            UpdateStatus("就绪");
        }

        private async void BtStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateSettings())
                {
                    MessageBox.Show("请检查配置参数", "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                BtStart.Enabled = false;
                txtDeviceId.Enabled = false;
                txtHost.Enabled = false;
                txtPort.Enabled = false;
                txtInterval.Enabled = false;

                await ConnectMqtt();

                _isRunning = true;
                BtStop.Enabled = true;

                int interval = int.Parse(txtInterval.Text) * 1000;
                timer.Interval = interval;
                timer.Start();

                await SendTemperature();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetUI();
            }
        }

        private async void BtStop_Click(object sender, EventArgs e)
        {
            try
            {
                _isRunning = false;
                timer.Stop();

                if (_mqttSubscriber is MqttClientService clientService)
                {
                    await clientService.DisconnectAsync();
                }

                UpdateStatus("已停止");
                toolStripStatusLabel.Text = "模拟测温设备 - 已停止";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ResetUI();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                _ = SendTemperature();
            }
        }

        private async Task SendTemperature()
        {
            try
            {
                double temperature = GenerateTemperature();
                LblTemperature.Text = temperature.ToString("F2");

                var data = new TemperatureData
                {
                    DeviceId = txtDeviceId.Text,
                    Temperature = temperature,
                    Timestamp = TimeUtils.GetUnixTime()
                };

                string payload = JsonConvert.SerializeObject(data);
                //string topic = $"sensors/temperature/{txtDeviceId.Text}";
                string topic = $"test/topic";

                await _mqttPublisher.PublishAsync(topic, payload, MqttQualityOfServiceLevel.AtLeastOnce);

                LblLastSend.Text = DateTime.Now.ToString("HH:mm:ss");
                toolStripStatusLabel.Text = $"已发送: {temperature:F2}°C";
            }
            catch (Exception ex)
            {
                UpdateStatus($"发送失败: {ex.Message}");
                toolStripStatusLabel.Text = $"发送失败: {ex.Message}";
            }
        }

        private async Task ConnectMqtt()
        {
            // 创建配置
            var config = new ClientConfig
            {
                Host = txtHost.Text,
                Port = int.Parse(txtPort.Text),
                ClientId = $"temp-device-{txtDeviceId.Text}-{Guid.NewGuid():N}",
                CleanSession = true,
                KeepAlivePeriod = 60,
                ReconnectDelay = 5,
            };

            // 创建服务实例
            var clientService = new MqttClientService(config);
            _mqttPublisher = clientService;
            _mqttSubscriber = clientService;

            // 先连接到 Broker
            await clientService.ConnectAsync();
            UpdateStatus("已连接");

            // 连接成功后再订阅响应消息
            string responseTopic = $"sensors/temperature/response";
            //string responseTopic = "test/topic";
            await _mqttSubscriber.SubscribeAsync(responseTopic, MqttQualityOfServiceLevel.AtLeastOnce);

            // 注册消息接收回调
            _mqttSubscriber.OnMessageReceived(async (topic, payload, qos) =>
            {
                Invoke((Action)(() =>
                {
                    toolStripStatusLabel.Text = $"收到消息: {topic}";
                    Console.WriteLine($"[WinForm] 收到响应: {payload}");
                }));
            });

            toolStripStatusLabel.Text = "模拟测温设备 - 运行中";
        }

        private double GenerateTemperature()
        {
            double variation = _random.NextDouble() * 2 - 1;
            return Math.Round(_baseTemperature + variation, 2);
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(txtDeviceId.Text))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHost.Text))
            {
                return false;
            }

            var text = txtPort.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            if (!TextUtils.IsInteger(text))
            {
                return false;
            }
            var port = int.Parse(text);
            if (port <= 0 || port > 65535)
            {
                return false;
            }

            text = txtInterval.Text.Trim();
            if (!TextUtils.IsInteger(text))
            {
                return false;
            }
            var interval = int.Parse(text);
            if (interval <= 0)
            {
                return false;
            }

            return true;
        }

        private void UpdateStatus(string status)
        {
            LblStatus.Text = $"状态：{status}";
        }

        private void ResetUI()
        {
            BtStart.Enabled = true;
            BtStop.Enabled = false;
            txtDeviceId.Enabled = true;
            txtHost.Enabled = true;
            txtPort.Enabled = true;
            txtInterval.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRunning = false;
            timer.Stop();
            if (_mqttSubscriber is MqttClientService clientService)
            {
                clientService.Dispose();
            }
        }
    }
}